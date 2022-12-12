using System.ComponentModel.DataAnnotations;
using VetClinic.Core.CustomAttributes;
using VetClinic.Data.Enums;
using static VetClinic.Data.ModelConstants.Pet;
using static VetClinic.Common.GlobalConstants.FormattingConstants;
using VetClinic.Core.Models.PetTypes;
using System.Globalization;

namespace VetClinic.Core.Models.Pets
{
    public class PetFormModel
    {
        [Required]
        [StringLength(NameMaxLength,MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [IsBeforeAttribute(MaxDate, ErrorMessage = "The Date of Birth must be before the current date")]
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow.Date;

        [Required]
        [StringLength (BreedMaxLength,MinimumLength = BreedMinLength)]
        public string Breed { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [StringLength(DescriptionMaxLength,MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Display(Name = "Pet Type")]
        public int PetTypeId { get; set; }
        public IEnumerable<PetTypeServiceModel> PetTypes { get; set; }
    }
}
