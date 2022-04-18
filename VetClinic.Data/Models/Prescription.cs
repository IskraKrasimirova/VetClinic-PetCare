using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VetClinic.Data.ModelConstants.Prescription;

namespace VetClinic.Data.Models
{
    public class Prescription
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        [ForeignKey(nameof(Pet))]
        public string PetId { get; set; }
        public Pet Pet { get; set; }

        [Required]
        [ForeignKey(nameof(Appointment))]
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public bool IsPublished { get; set; }
    }
}
