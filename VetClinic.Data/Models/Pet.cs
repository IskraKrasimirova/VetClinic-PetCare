using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Data.Enums;
using static VetClinic.Data.ModelConstants.Pet;

namespace VetClinic.Data.Models
{
    public class Pet
    {
        public Pet()
        {
            Id = Guid.NewGuid().ToString();
            PetServices = new HashSet<PetService>();
            PetDoctors = new HashSet<PetDoctor>();    
            Prescriptions = new HashSet<Prescription>();
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(PetType))]
        public int PetTypeId { get; set; }
        public PetType PetType { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Column(TypeName ="date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(BreedMaxLength)]
        public string Breed { get; set; }

        public Gender Gender { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<PetService> PetServices { get; set; }
        public ICollection<PetDoctor> PetDoctors { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
