using ClinicService.Models;
using ClinicService.Models.Requests.Authentication;

namespace ClinicService.Services.Interfaces;

public interface IAuthenticateService
{
    AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

    public SessionInfo GetSessionInfo(string sessionToken);
}