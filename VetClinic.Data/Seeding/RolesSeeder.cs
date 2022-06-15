using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VetClinic.Data.Seeding.Contracts;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Data.Seeding
{
    public class RolesSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdminRoleName };

                    await roleManager.CreateAsync(role);

                    if (await roleManager.RoleExistsAsync(DoctorRoleName))
                    {
                        return;
                    }

                    var doctorRole = new IdentityRole { Name = DoctorRoleName };

                    await roleManager.CreateAsync(doctorRole);

                    if (await roleManager.RoleExistsAsync(ClientRoleName))
                    {
                        return;
                    }

                    var clientRole = new IdentityRole { Name = ClientRoleName };

                    await roleManager.CreateAsync(clientRole);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
