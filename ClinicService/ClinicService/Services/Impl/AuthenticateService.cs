using ClinicService.Data.Context;
using ClinicService.Data.Models;
using ClinicService.Models;
using ClinicService.Models.Requests.Authentication;
using ClinicService.Services.Interfaces;
using ClinicService.Sesurity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicService.Services.Impl;

public class AuthenticateService : IAuthenticateService
{

    #region Services

    private readonly IServiceScopeFactory _serviceScopeFactory;

    #endregion

    /// <summary> Словарь сессий. </summary>
    /// <remarks> Используется для ускорения работы (меньше обращений к БД). </remarks>
    private readonly Dictionary<string, SessionInfo> _sessions = new Dictionary<string, SessionInfo>();

    /// <summary> Ключ для работы с jwt-токенами. </summary>
    public const string SecretKey = "kYp3s6v9y/B?E(H+";

    /// <summary> Ctor. </summary>
    /// <param name="serviceScopeFactory"></param>
    public AuthenticateService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public SessionInfo GetSessionInfo(string sessionToken)
    {
        SessionInfo sessionInfo;

        lock (_sessions)
        {
            _sessions.TryGetValue(sessionToken, out sessionInfo!);
        }

        if (sessionInfo is null)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ClinicServiceDbContext context = scope.ServiceProvider.GetRequiredService<ClinicServiceDbContext>();

            AccountSession session = context.AccountSessions.FirstOrDefault(item => item.SessionToken == sessionToken)!;

            if (session is null)
                return null!;

            Account account = context.Accounts.FirstOrDefault(item => item.AccountId == session.AccountId)!;

            sessionInfo = GetSessionInfo(account, session);

            if (sessionInfo is not null)
            {
                lock (_sessions)
                {
                    _sessions[sessionToken] = sessionInfo;
                }
            }
        }

        return sessionInfo!;
    }

    public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        ClinicServiceDbContext context = scope.ServiceProvider.GetRequiredService<ClinicServiceDbContext>();

        Account account = !string.IsNullOrWhiteSpace(authenticationRequest.Login) ? FindAccountByLogin(context, authenticationRequest.Login) : null!;

        if (account is null)
        {
            return new AuthenticationResponse
            {
                Status = AuthenticationStatus.UserNotFound
            };
        }

        //проверка пароля
        if (!PasswordUtils.VerifyPassword(authenticationRequest.Password, account.PasswordSalt, account.PasswordHash))
        {
            return new AuthenticationResponse
            {
                Status = AuthenticationStatus.InvalidPassword
            };
        }

        //новая сессия
        AccountSession session = new AccountSession
        {
            AccountId = account.AccountId,
            SessionToken = CreateSessionToken(account),
            TimeCreated = DateTime.Now,
            TimeLastRequest = DateTime.Now,
            IsClosed = false,
        };

        //сохранение сессии в БД.
        context.AccountSessions.Add(session);
        context.SaveChanges();


        SessionInfo sessionInfo = GetSessionInfo(account, session);

        lock (_sessions)
        {
            _sessions[sessionInfo.SessionToken] = sessionInfo;
        }

        return new AuthenticationResponse
        {
            Status = AuthenticationStatus.Success,
            SessionInfo = sessionInfo
        };
    }

    /// <summary> Получить объект SessionInfo. </summary>
    /// <param name="account"> Аккаунт для которого создается объект. </param>
    /// <param name="accountSession"> Сессия аккаунта. </param>
    /// <returns> Объект <see cref="SessionInfo"/> </returns>
    private SessionInfo GetSessionInfo(Account account, AccountSession accountSession)
    {
        return new SessionInfo
        {
            SessionId = accountSession.SessionId,
            SessionToken = accountSession.SessionToken,
            Account = new AccountDto
            {
                AccountId = account.AccountId,
                EMail = account.EMail,
                FirstName = account.FirstName,
                LastName = account.LastName,
                SecondName = account.SecondName,
                Locked = account.Locked
            }
        };
    }

    /// <summary> Создать токен. </summary>
    /// <param name="account"> Аккаунт пользователя. </param>
    /// <returns> Строка токена. </returns>
    private string CreateSessionToken(Account account)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(SecretKey);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(   //информация о пользователе
                new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()), //идентификатор пользователя
                    new Claim(ClaimTypes.Name, account.EMail),  //email клиента
                }),
            Expires = DateTime.UtcNow.AddMinutes(15),   //время жизни
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)  //объект с информацией о ключе
        };

        //генерация токена
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary> Найти аккаунт по логину. </summary>
    /// <param name="context"> Контекст БД. </param>
    /// <param name="login"> Логин. </param>
    /// <returns> Аккаунт <see cref="Account"/>. </returns>
    private Account FindAccountByLogin(ClinicServiceDbContext context, string login)
    {
        return context.Accounts.FirstOrDefault(account => account.EMail == login)!;
    }
}
