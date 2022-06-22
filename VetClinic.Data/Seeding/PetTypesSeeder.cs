using VetClinic.Data.Models;
using VetClinic.Data.Seeding.Contracts;

namespace VetClinic.Data.Seeding
{
    public class PetTypesSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            if (!data.PetTypes.Any())
            {
                var petTypes = new List<PetType>
                { 
                    new PetType
                    {
                        Name = "Dog"
                    },
                    new PetType
                    {
                        Name = "Cat"
                    },
                    new PetType
                    {
                        Name = "Bird"
                    },
                    new PetType
                    {
                        Name = "Rodent"
                    },
                    new PetType
                    {
                        Name = "Reptile"
                    },
                    new PetType
                    {
                        Name = "Exotic"
                    },
                    new PetType
                    {
                        Name = "Fish"
                    },
                    new PetType
                    {
                        Name = "Other"
                    }
                };

                data.PetTypes.AddRange(petTypes);
                data.SaveChanges();
            }
        }
    }
}
