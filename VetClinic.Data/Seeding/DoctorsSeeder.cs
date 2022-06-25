using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VetClinic.Data.Models;
using VetClinic.Data.Seeding.Contracts;
using VetClinic.Data.Seeding.SeedData;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Data.Seeding
{
    public class DoctorsSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();

            Task
                .Run(async () =>
                {
                    if (!data.Doctors.Any())
                    {
                        var allDoctors = new List<Doctor>();

                        await SeedConsultationDoctor1(userManager, allDoctors);
                        await SeedConsultationDoctor2(userManager, allDoctors);
                        await SeedConsultationDoctor3(userManager, allDoctors);
                        await SeedInternalDoctor1(userManager, allDoctors);
                        await SeedInternalDoctor2(userManager, allDoctors);
                        await SeedInternalDoctor3(userManager, allDoctors);
                        await SeedSurgeryDoctor1(userManager, allDoctors);
                        await SeedSurgeryDoctor2(userManager, allDoctors);
                        await SeedOrthopedicsDoctor1(userManager, allDoctors);
                        await SeedDermatologyDoctor1(userManager, allDoctors);
                        await SeedEmergencyDoctor1(userManager, allDoctors);
                        await SeedEmergencyDoctor2(userManager, allDoctors);
                        await SeedGroomingDoctor1(userManager, allDoctors);

                        await data.Doctors.AddRangeAsync(allDoctors);
                    }

                    await data.SaveChangesAsync();
                })
                .GetAwaiter()
                .GetResult();
        }

        private static async Task SeedConsultationDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var doctor1 = new User()
            {
                Email = DoctorsSeedData.ConsultationDoctor1.Email,
                UserName = DoctorsSeedData.ConsultationDoctor1.Username,
                PhoneNumber = DoctorsSeedData.ConsultationDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.ConsultationDoctor1.FullName,
            };

            await userManager.CreateAsync
                (doctor1, DoctorsSeedData.ConsultationDoctor1.Password);

            await userManager.AddToRoleAsync(doctor1, DoctorRoleName);

            var newDoctor1 = new Doctor()
            {
                UserId = doctor1.Id,
                FullName = DoctorsSeedData.ConsultationDoctor1.FullName,
                Email = DoctorsSeedData.ConsultationDoctor1.Email,
                PhoneNumber = DoctorsSeedData.ConsultationDoctor1.PhoneNumber,
                Description = DoctorsSeedData.ConsultationDoctor1.Description,
                DepartmentId = DoctorsSeedData.ConsultationDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.ConsultationDoctor1.ProfileImage
            };

            allDoctors.Add(newDoctor1);
        }

        private static async Task SeedConsultationDoctor2
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var doctor2 = new User()
            {
                Email = DoctorsSeedData.ConsultationDoctor2.Email,
                UserName = DoctorsSeedData.ConsultationDoctor2.Username,
                PhoneNumber = DoctorsSeedData.ConsultationDoctor2.PhoneNumber,
                FullName = DoctorsSeedData.ConsultationDoctor2.FullName,
            };

            await userManager.CreateAsync
                (doctor2, DoctorsSeedData.ConsultationDoctor2.Password);

            await userManager.AddToRoleAsync(doctor2, DoctorRoleName);

            var newDoctor2 = new Doctor()
            {
                UserId = doctor2.Id,
                FullName = DoctorsSeedData.ConsultationDoctor2.FullName,
                Email = DoctorsSeedData.ConsultationDoctor2.Email,
                PhoneNumber = DoctorsSeedData.ConsultationDoctor2.PhoneNumber,
                Description = DoctorsSeedData.ConsultationDoctor2.Description,
                DepartmentId = DoctorsSeedData.ConsultationDoctor2.DepartmentId,
                ProfileImage = DoctorsSeedData.ConsultationDoctor2.ProfileImage
            };

            allDoctors.Add(newDoctor2);
        }

        private static async Task SeedConsultationDoctor3
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var doctor3 = new User()
            {
                Email = DoctorsSeedData.ConsultationDoctor3.Email,
                UserName = DoctorsSeedData.ConsultationDoctor3.Username,
                PhoneNumber = DoctorsSeedData.ConsultationDoctor3.PhoneNumber,
                FullName = DoctorsSeedData.ConsultationDoctor3.FullName,
            };

            await userManager.CreateAsync
                (doctor3, DoctorsSeedData.ConsultationDoctor3.Password);

            await userManager.AddToRoleAsync(doctor3, DoctorRoleName);

            var newDoctor3 = new Doctor()
            {
                UserId = doctor3.Id,
                FullName = DoctorsSeedData.ConsultationDoctor3.FullName,
                Email = DoctorsSeedData.ConsultationDoctor3.Email,
                PhoneNumber = DoctorsSeedData.ConsultationDoctor3.PhoneNumber,
                Description = DoctorsSeedData.ConsultationDoctor3.Description,
                DepartmentId = DoctorsSeedData.ConsultationDoctor3.DepartmentId,
                ProfileImage = DoctorsSeedData.ConsultationDoctor3.ProfileImage
            };

            allDoctors.Add(newDoctor3);
        }

        private static async Task SeedInternalDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var internalDoctor1 = new User()
            {
                Email = DoctorsSeedData.InternalDoctor1.Email,
                UserName = DoctorsSeedData.InternalDoctor1.Username,
                PhoneNumber = DoctorsSeedData.InternalDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.InternalDoctor1.FullName,
            };

            await userManager.CreateAsync
                (internalDoctor1, DoctorsSeedData.InternalDoctor1.Password);

            await userManager.AddToRoleAsync(internalDoctor1, DoctorRoleName);

            var newInternalDoctor1 = new Doctor()
            {
                UserId = internalDoctor1.Id,
                FullName = DoctorsSeedData.InternalDoctor1.FullName,
                Email = DoctorsSeedData.InternalDoctor1.Email,
                PhoneNumber = DoctorsSeedData.InternalDoctor1.PhoneNumber,
                Description = DoctorsSeedData.InternalDoctor1.Description,
                DepartmentId = DoctorsSeedData.InternalDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.InternalDoctor1.ProfileImage
            };

            allDoctors.Add(newInternalDoctor1);
        }

        private static async Task SeedInternalDoctor2
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var internalDoctor2 = new User()
            {
                Email = DoctorsSeedData.InternalDoctor2.Email,
                UserName = DoctorsSeedData.InternalDoctor2.Username,
                PhoneNumber = DoctorsSeedData.InternalDoctor2.PhoneNumber,
                FullName = DoctorsSeedData.InternalDoctor2.FullName,
            };

            await userManager.CreateAsync
                (internalDoctor2, DoctorsSeedData.InternalDoctor2.Password);

            await userManager.AddToRoleAsync(internalDoctor2, DoctorRoleName);

            var newInternalDoctor2 = new Doctor()
            {
                UserId = internalDoctor2.Id,
                FullName = DoctorsSeedData.InternalDoctor2.FullName,
                Email = DoctorsSeedData.InternalDoctor2.Email,
                PhoneNumber = DoctorsSeedData.InternalDoctor2.PhoneNumber,
                Description = DoctorsSeedData.InternalDoctor2.Description,
                DepartmentId = DoctorsSeedData.InternalDoctor2.DepartmentId,
                ProfileImage = DoctorsSeedData.InternalDoctor2.ProfileImage
            };

            allDoctors.Add(newInternalDoctor2);
        }

        private static async Task SeedInternalDoctor3
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var internalDoctor3 = new User()
            {
                Email = DoctorsSeedData.InternalDoctor3.Email,
                UserName = DoctorsSeedData.InternalDoctor3.Username,
                PhoneNumber = DoctorsSeedData.InternalDoctor3.PhoneNumber,
                FullName = DoctorsSeedData.InternalDoctor3.FullName,
            };

            await userManager.CreateAsync
                (internalDoctor3, DoctorsSeedData.InternalDoctor3.Password);

            await userManager.AddToRoleAsync(internalDoctor3, DoctorRoleName);

            var newInternalDoctor3 = new Doctor()
            {
                UserId = internalDoctor3.Id,
                FullName = DoctorsSeedData.InternalDoctor3.FullName,
                Email = DoctorsSeedData.InternalDoctor3.Email,
                PhoneNumber = DoctorsSeedData.InternalDoctor3.PhoneNumber,
                Description = DoctorsSeedData.InternalDoctor3.Description,
                DepartmentId = DoctorsSeedData.InternalDoctor3.DepartmentId,
                ProfileImage = DoctorsSeedData.InternalDoctor3.ProfileImage
            };

            allDoctors.Add(newInternalDoctor3);
        }

        private static async Task SeedSurgeryDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var surgeryDoctor1 = new User()
            {
                Email = DoctorsSeedData.SurgeryDoctor1.Email,
                UserName = DoctorsSeedData.SurgeryDoctor1.Username,
                PhoneNumber = DoctorsSeedData.SurgeryDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.SurgeryDoctor1.FullName,
            };

            await userManager.CreateAsync
                (surgeryDoctor1, DoctorsSeedData.SurgeryDoctor1.Password);

            await userManager.AddToRoleAsync(surgeryDoctor1, DoctorRoleName);

            var newSurgeryDoctor1 = new Doctor()
            {
                UserId = surgeryDoctor1.Id,
                FullName = DoctorsSeedData.SurgeryDoctor1.FullName,
                Email = DoctorsSeedData.SurgeryDoctor1.Email,
                PhoneNumber = DoctorsSeedData.SurgeryDoctor1.PhoneNumber,
                Description = DoctorsSeedData.SurgeryDoctor1.Description,
                DepartmentId = DoctorsSeedData.SurgeryDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.SurgeryDoctor1.ProfileImage
            };

            allDoctors.Add(newSurgeryDoctor1);
        }

        private static async Task SeedSurgeryDoctor2
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var surgeryDoctor2 = new User()
            {
                Email = DoctorsSeedData.SurgeryDoctor2.Email,
                UserName = DoctorsSeedData.SurgeryDoctor2.Username,
                PhoneNumber = DoctorsSeedData.SurgeryDoctor2.PhoneNumber,
                FullName = DoctorsSeedData.SurgeryDoctor2.FullName,
            };

            await userManager.CreateAsync
                (surgeryDoctor2, DoctorsSeedData.SurgeryDoctor2.Password);

            await userManager.AddToRoleAsync(surgeryDoctor2, DoctorRoleName);

            var newSurgeryDoctor2 = new Doctor()
            {
                UserId = surgeryDoctor2.Id,
                FullName = DoctorsSeedData.SurgeryDoctor2.FullName,
                Email = DoctorsSeedData.SurgeryDoctor2.Email,
                PhoneNumber = DoctorsSeedData.SurgeryDoctor2.PhoneNumber,
                Description = DoctorsSeedData.SurgeryDoctor2.Description,
                DepartmentId = DoctorsSeedData.SurgeryDoctor2.DepartmentId,
                ProfileImage = DoctorsSeedData.SurgeryDoctor2.ProfileImage
            };

            allDoctors.Add(newSurgeryDoctor2);
        }

        private static async Task SeedOrthopedicsDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var orthopedicsDoctor1 = new User()
            {
                Email = DoctorsSeedData.OrthopedicsDoctor1.Email,
                UserName = DoctorsSeedData.OrthopedicsDoctor1.Username,
                PhoneNumber = DoctorsSeedData.OrthopedicsDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.OrthopedicsDoctor1.FullName,
            };

            await userManager.CreateAsync
                (orthopedicsDoctor1, DoctorsSeedData.OrthopedicsDoctor1.Password);

            await userManager.AddToRoleAsync(orthopedicsDoctor1, DoctorRoleName);

            var newOrthopedicsDoctor1 = new Doctor()
            {
                UserId = orthopedicsDoctor1.Id,
                FullName = DoctorsSeedData.OrthopedicsDoctor1.FullName,
                Email = DoctorsSeedData.OrthopedicsDoctor1.Email,
                PhoneNumber = DoctorsSeedData.OrthopedicsDoctor1.PhoneNumber,
                Description = DoctorsSeedData.OrthopedicsDoctor1.Description,
                DepartmentId = DoctorsSeedData.OrthopedicsDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.OrthopedicsDoctor1.ProfileImage
            };

            allDoctors.Add(newOrthopedicsDoctor1);
        }

        private static async Task SeedDermatologyDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var dermatologyDoctor1 = new User()
            {
                Email = DoctorsSeedData.DermatologyDoctor1.Email,
                UserName = DoctorsSeedData.DermatologyDoctor1.Username,
                PhoneNumber = DoctorsSeedData.DermatologyDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.DermatologyDoctor1.FullName,
            };

            await userManager.CreateAsync
                (dermatologyDoctor1, DoctorsSeedData.DermatologyDoctor1.Password);

            await userManager.AddToRoleAsync(dermatologyDoctor1, DoctorRoleName);

            var newDermatologyDoctor1 = new Doctor()
            {
                UserId = dermatologyDoctor1.Id,
                FullName = DoctorsSeedData.DermatologyDoctor1.FullName,
                Email = DoctorsSeedData.DermatologyDoctor1.Email,
                PhoneNumber = DoctorsSeedData.DermatologyDoctor1.PhoneNumber,
                Description = DoctorsSeedData.DermatologyDoctor1.Description,
                DepartmentId = DoctorsSeedData.DermatologyDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.DermatologyDoctor1.ProfileImage
            };

            allDoctors.Add(newDermatologyDoctor1);
        }

        private static async Task SeedEmergencyDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var emergencyDoctor1 = new User()
            {
                Email = DoctorsSeedData.EmergencyDoctor1.Email,
                UserName = DoctorsSeedData.EmergencyDoctor1.Username,
                PhoneNumber = DoctorsSeedData.EmergencyDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.EmergencyDoctor1.FullName,
            };

            await userManager.CreateAsync
                (emergencyDoctor1, DoctorsSeedData.EmergencyDoctor1.Password);

            await userManager.AddToRoleAsync(emergencyDoctor1, DoctorRoleName);

            var newEmergencyDoctor1 = new Doctor()
            {
                UserId = emergencyDoctor1.Id,
                FullName = DoctorsSeedData.EmergencyDoctor1.FullName,
                Email = DoctorsSeedData.EmergencyDoctor1.Email,
                PhoneNumber = DoctorsSeedData.EmergencyDoctor1.PhoneNumber,
                Description = DoctorsSeedData.EmergencyDoctor1.Description,
                DepartmentId = DoctorsSeedData.EmergencyDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.EmergencyDoctor1.ProfileImage
            };

            allDoctors.Add(newEmergencyDoctor1);
        }

        private static async Task SeedEmergencyDoctor2
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var emergencyDoctor2 = new User()
            {
                Email = DoctorsSeedData.EmergencyDoctor2.Email,
                UserName = DoctorsSeedData.EmergencyDoctor2.Username,
                PhoneNumber = DoctorsSeedData.EmergencyDoctor2.PhoneNumber,
                FullName = DoctorsSeedData.EmergencyDoctor2.FullName,
            };

            await userManager.CreateAsync
                (emergencyDoctor2, DoctorsSeedData.EmergencyDoctor2.Password);

            await userManager.AddToRoleAsync(emergencyDoctor2, DoctorRoleName);

            var newEmergencyDoctor2 = new Doctor()
            {
                UserId = emergencyDoctor2.Id,
                FullName = DoctorsSeedData.EmergencyDoctor2.FullName,
                Email = DoctorsSeedData.EmergencyDoctor2.Email,
                PhoneNumber = DoctorsSeedData.EmergencyDoctor2.PhoneNumber,
                Description = DoctorsSeedData.EmergencyDoctor2.Description,
                DepartmentId = DoctorsSeedData.EmergencyDoctor2.DepartmentId,
                ProfileImage = DoctorsSeedData.EmergencyDoctor2.ProfileImage
            };

            allDoctors.Add(newEmergencyDoctor2);
        }

        private static async Task SeedGroomingDoctor1
            (UserManager<User> userManager, ICollection<Doctor> allDoctors)
        {
            var groomingDoctor1 = new User()
            {
                Email = DoctorsSeedData.GroomingDoctor1.Email,
                UserName = DoctorsSeedData.GroomingDoctor1.Username,
                PhoneNumber = DoctorsSeedData.GroomingDoctor1.PhoneNumber,
                FullName = DoctorsSeedData.GroomingDoctor1.FullName,
            };

            await userManager.CreateAsync
                (groomingDoctor1, DoctorsSeedData.GroomingDoctor1.Password);

            await userManager.AddToRoleAsync(groomingDoctor1, DoctorRoleName);

            var newGroomingDoctor1 = new Doctor()
            {
                UserId = groomingDoctor1.Id,
                FullName = DoctorsSeedData.GroomingDoctor1.FullName,
                Email = DoctorsSeedData.GroomingDoctor1.Email,
                PhoneNumber = DoctorsSeedData.GroomingDoctor1.PhoneNumber,
                Description = DoctorsSeedData.GroomingDoctor1.Description,
                DepartmentId = DoctorsSeedData.GroomingDoctor1.DepartmentId,
                ProfileImage = DoctorsSeedData.GroomingDoctor1.ProfileImage
            };

            allDoctors.Add(newGroomingDoctor1);
        }
    }
}
