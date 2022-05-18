using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static VetClinic.Data.ModelConstants.Doctor;

namespace VetClinic.Data.Models
{
    public class Doctor
    {
        public Doctor()
        {
            Id = Guid.NewGuid().ToString();
            DoctorServices = new HashSet<DoctorService>();
            PetDoctors = new HashSet<PetDoctor>();
            Prescriptions = new HashSet<Prescription>();
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }


        [Required]
        public string ProfileImage { get; set; }

        [Required]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public string UserId { get; set; }

        public ICollection<DoctorService> DoctorServices { get; set; }
        public ICollection<PetDoctor> PetDoctors { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
