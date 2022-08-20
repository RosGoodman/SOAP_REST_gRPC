namespace ClinicService.Models.Requests.Consultation;

public class UpdateConsultationRequest
{
    public int ConsultationId { get; set; }

    public string? Descripion { get; set; }

    public DateTime ConsultationDate { get; set; }

    public int ClientId { get; set; }

    public int PetId { get; set; }
}
