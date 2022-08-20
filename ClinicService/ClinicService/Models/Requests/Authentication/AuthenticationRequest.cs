#nullable disable
namespace ClinicService.Models.Requests.Authentication;

public class AuthenticationRequest
{
    public string Login { get; set; }

    public string Password { get; set; }
}
