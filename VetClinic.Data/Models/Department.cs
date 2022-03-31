using System.ComponentModel.DataAnnotations;
using static VetClinic.Data.ModelConstants.Departmenet;

namespace VetClinic.Data.Models
{
    public class Department
    {
        public Department()
        {
            Services = new HashSet<Service>();
            Doctors = new HashSet<Doctor>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public string Image { get; set; }

        public ICollection<Service> Services { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
    }
}
