using System.ComponentModel.DataAnnotations;
using VetClinic.Data.Enums;
using static VetClinic.Data.ModelConstants.Pet;

namespace VetClinic.Core.Models.Pets
{
    public class AddPetFormModel
    {
        [Required]
        [StringLength(NameMaxLength,MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength (BreedMaxLength,MinimumLength = BreedMinLength)]
        public string Breed { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [StringLength(DescriptionMaxLength,MinimumLength = DescriptionMinLength)]
        public string? Description { get; set; }

        [Display(Name = "Pet Type")]
        public int PetTypeId { get; set; }
        public ICollection<PetTypeServiceModel>? PetTypes { get; set; }
    }
}
