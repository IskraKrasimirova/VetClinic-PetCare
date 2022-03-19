using System.ComponentModel.DataAnnotations;
using static VetClinic.Data.ModelConstants.PetType;

namespace VetClinic.Data.Models
{
    public class PetType
    {
        public PetType()
        {
            Pets = new HashSet<Pet>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}
