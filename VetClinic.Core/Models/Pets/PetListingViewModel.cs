using System.ComponentModel.DataAnnotations;

namespace VetClinic.Core.Models.Pets
{
    public class PetListingViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DateOfBirth { get; set; }

        public string Breed { get; set; }

        public string Gender { get; set; }

        public string PetType { get; set; }
        public string Description { get; set; }
    }
}
