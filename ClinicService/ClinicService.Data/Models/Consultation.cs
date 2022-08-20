#nullable disable

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicService.Data.Models;

[Table("Consultations")]
public class Consultation
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConsultationId { get; set; }

    [Column]
    public DateTime ConsultationDate { get; set; }

    [Column]
    public string Description { get; set; }


    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }

    public virtual Client Client { get; set; }


    [ForeignKey(nameof(Pet))]
    public int PetId { get; set; }

    public virtual Pet Pet { get; set; }
}
