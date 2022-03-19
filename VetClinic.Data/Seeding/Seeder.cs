using VetClinic.Data.Seeding.Contracts;
using VetClinic.Data.Seeding.SeedData;

namespace VetClinic.Data.Seeding
{
    public class Seeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            var seeders = new List<ISeeder>()
                { new PetTypesSeeder(),
                    new DepartmentsSeeder(),
                    new RolesSeeder(),
                    new UsersSeeder(),
                    new DoctorsSeeder(),
                    new ServicesSeeder(),
                };

            foreach (var seeder in seeders)
            {
                seeder.Seed(data, serviceProvider);
            }
        }
    }
}
