using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VetClinic.Data.ModelConstants.Service;

namespace VetClinic.Data.Models
{
    public class Service
    {
        public Service()
        {
            PetServices = new HashSet<PetService>();
            DoctorServices = new HashSet<DoctorService>();
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public ICollection<PetService> PetServices { get; set; }
        public ICollection<DoctorService> DoctorServices { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
