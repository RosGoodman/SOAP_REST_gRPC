namespace ClinicService.Models.Requests.Consultation;

public class CreateConsultationRequest
{
    public string? Descripion { get; set; }

    public DateTime ConsultationDate { get; set; }

    public int ClientId { get; set; }

    public int PetId { get; set; }
}
