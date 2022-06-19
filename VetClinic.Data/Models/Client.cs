using System.ComponentModel.DataAnnotations;
using static VetClinic.Data.ModelConstants.Client;

namespace VetClinic.Data.Models
{
    public class Client 
    {
        public Client()
        {
            Id = Guid.NewGuid().ToString();
            Pets = new HashSet<Pet>();
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<Pet> Pets { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
