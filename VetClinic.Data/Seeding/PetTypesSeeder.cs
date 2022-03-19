using VetClinic.Data.Models;
using VetClinic.Data.Seeding.Contracts;
using VetClinic.Data.Seeding.SeedData;

namespace VetClinic.Data.Seeding
{
    public class PetTypesSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            if (!data.PetTypes.Any())
            {
                var allPetTypes = new List<PetType>();
                var petTypesData = new PetTypesSeedData();
                foreach (var petTypeName in PetTypesSeedData.PetTypes)
                {
                    var petType = new PetType() { Name = petTypeName };
                    allPetTypes.Add(petType);
                }

                data.PetTypes.AddRange(allPetTypes);
                data.SaveChanges();
            }
        }
    }
}
