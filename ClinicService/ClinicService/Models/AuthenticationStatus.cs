namespace ClinicService.Models;

/// <summary> Перечисление статусов аутентификации. </summary>
public enum AuthenticationStatus
{
    Success = 0,
    UserNotFound = 1,
    InvalidPassword = 2
}
