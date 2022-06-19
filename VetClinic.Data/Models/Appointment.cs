using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinic.Data.Models
{
    public class Appointment
    {
        public Appointment()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public string Hour { get; set; }

        [Required]
        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Required]
        [ForeignKey(nameof(Pet))]
        public string PetId { get; set; }
        public Pet Pet { get; set; }

        [ForeignKey(nameof(Prescription))]
        public string PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
    }
}
