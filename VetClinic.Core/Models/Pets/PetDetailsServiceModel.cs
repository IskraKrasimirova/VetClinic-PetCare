namespace VetClinic.Core.Models.Pets
{
    public class PetDetailsServiceModel : PetListingViewModel
    {
        public string Description { get; set; }

        public int PetTypeId { get; set; }

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string UserId { get; set; }
    }
}
