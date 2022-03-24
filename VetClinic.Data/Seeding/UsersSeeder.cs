using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VetClinic.Data.Models;
using VetClinic.Data.Seeding.Contracts;
using VetClinic.Data.Seeding.SeedData;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Data.Seeding
{
    public class UsersSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();

            Task
                .Run(async () =>
                {
                    if (!data.Users.Any
                        (u => u.UserName == UsersSeedData.Admin.Username))
                        await SeedAdmin(userManager);

                    if (!data.Users.Any
                        (u => u.UserName == UsersSeedData.Client.Username))
                        await SeedClient(data, userManager);

                    if (!data.Users.Any
                        (u => u.UserName == UsersSeedData.Doctor.Username))
                        await SeedDoctor(data, userManager);

                    await data.SaveChangesAsync();
                })
                .GetAwaiter()
                .GetResult();
        }

        private static async Task SeedAdmin
            (UserManager<User> userManager)
        {
            var admin = new User()
            {
                Email = UsersSeedData.Admin.Email,
                UserName = UsersSeedData.Admin.Username,
                FullName = UsersSeedData.Admin.FullName,
                PhoneNumber = UsersSeedData.Admin.PhoneNumber,
            };

            await userManager.CreateAsync
                (admin, UsersSeedData.Admin.Password);

            await userManager.AddToRoleAsync(admin, AdminRoleName);
        }

        private static async Task SeedClient
            (VetClinicDbContext data, UserManager<User> userManager)
        {
            var client = new User()
            {
                Email = UsersSeedData.Client.Email,
                UserName = UsersSeedData.Client.Username,
                PhoneNumber = UsersSeedData.Client.PhoneNumber,
                FullName = UsersSeedData.Client.FullName
            };

            await userManager.CreateAsync
                (client, UsersSeedData.Client.Password);

            await userManager.AddToRoleAsync(client, ClientRoleName);

            var newClient = new Client()
            {
                UserId = client.Id,
                FullName = UsersSeedData.Client.FullName,
            };

            data.Clients.Add(newClient);
        }

        private static async Task SeedDoctor
            (VetClinicDbContext data, UserManager<User> userManager)
        {
            var doctor = new User()
            {
                Email = UsersSeedData.Doctor.Email,
                UserName = UsersSeedData.Doctor.Username,
                PhoneNumber = UsersSeedData.Doctor.PhoneNumber,
                FullName = UsersSeedData.Doctor.FullName,
            };

            await userManager.CreateAsync
                (doctor, UsersSeedData.Doctor.Password);

            await userManager.AddToRoleAsync(doctor, DoctorRoleName);

            var newDoctor = new Doctor()
            {
                UserId = doctor.Id,
                FullName = UsersSeedData.Doctor.FullName,
                Description = UsersSeedData.Doctor.Description,
                DepartmentId = UsersSeedData.Doctor.DepartmentId
            };

            data.Doctors.Add(newDoctor);
        }
    }
}
