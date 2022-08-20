#nullable disable

namespace ClinicService.Models.Requests.Authentication;

public class AuthenticationResponse
{
    public AuthenticationStatus Status { get; set; }

    public SessionInfo SessionInfo { get; set; }
}
