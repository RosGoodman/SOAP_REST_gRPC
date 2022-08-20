namespace ClinicService.Models.Requests.Pet;

public class CreatePetRequest
{
    public string? Name { get; set; }

    public DateTime Birthday { get; set; }

    public int ClientId { get; set; }
}
