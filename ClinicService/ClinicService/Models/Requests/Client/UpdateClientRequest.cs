namespace ClinicService.Models.Requests.Client;

public class UpdateClientRequest
{
    public int ClientId { get; set; }

    public string? Document { get; set; }

    public string? Surname { get; set; }

    public string? FirstName { get; set; }

    public string? Patronymic { get; set; }
}
