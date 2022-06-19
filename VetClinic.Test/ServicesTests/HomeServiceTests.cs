using NUnit.Framework;
using System.Linq;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;

namespace VetClinic.Test.ServicesTests
{
    public class HomeServiceTests
    {
        private VetClinicDbContext dbContext;
        private HomeService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new HomeService(dbContext);
        }

        [Test]
        public void GetAdminFullNameShouldWorkCorrectWhenAdminExists()
        {
            GetDbContextWithUsers();
            var result = service.GetAdminFullName("TestAdmintUserId");
            var expectedCount = dbContext.Users
                .Where(u => u.Id == "TestAdmintUserId")
                .Count();
            var expectedAdminFullName = dbContext.Users
                .First(u => u.Id == "TestAdmintUserId")
                .FullName;
            Assert.That(result.GetType(), Is.EqualTo(typeof(string)));
            Assert.That(result, Is.EqualTo(expectedAdminFullName));
            Assert.That(expectedCount == 1);
        }

        [Test]
        public void GetAdminFullNameShouldReturnNullWhenAdminNotExist()
        {
            GetDbContextWithUsers();
            var result = service.GetAdminFullName("NotExistingUserId");
            var expectedCount = dbContext.Users
                .Where(u => u.Id == "NotExistingUserId")
                .Count();
            Assert.IsNull(result);
            Assert.That(expectedCount == 0);
        }

        [Test]
        public void GetClientFullNameShouldWorkCorrectWhenClientExists()
        {
            GetDbContextWithUsers();
            var result = service.GetClientFullName("TestUserId3");
            var expectedCount = dbContext.Clients
                .Where(c => c.UserId == "TestUserId3")
                .Count();
            var expectedClientFullName = dbContext.Clients
                .First(c => c.UserId == "TestUserId3")
                .FullName;
            Assert.That(result.GetType(), Is.EqualTo(typeof(string)));
            Assert.That(result, Is.EqualTo(expectedClientFullName));
            Assert.That(expectedCount == 1);
        }

        [Test]
        public void GetClientFullNameShouldReturnNullWhenClientNotExist()
        {
            GetDbContextWithUsers();
            var result = service.GetClientFullName("NotExistingUserId");
            var expectedCount = dbContext.Clients
                .Where(c => c.UserId == "NotExistingUserId")
                .Count();
            Assert.IsNull(result);
            Assert.That(expectedCount == 0);
        }

        [Test]
        public void GetDoctorFullNameShouldWorkCorrectWhenDoctorExists()
        {
            GetDbContextWithUsers();
            var result = service.GetDoctorFullName("TestUserId");
            var expectedCount = dbContext.Doctors
                .Where(d => d.UserId == "TestUserId")
                .Count();
            var expectedDoctorFullName = dbContext.Doctors
                .First(d => d.UserId == "TestUserId")
                .FullName;
            Assert.That(result.GetType(), Is.EqualTo(typeof(string)));
            Assert.That(result, Is.EqualTo(expectedDoctorFullName));
            Assert.That(expectedCount == 1);
        }

        [Test]
        public void GetDoctorFullNameShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithUsers();
            var result = service.GetDoctorFullName("NotExistingUserId");
            var expectedCount = dbContext.Doctors
                .Where(d => d.UserId == "NotExistingUserId")
                .Count();
            Assert.IsNull(result);
            Assert.That(expectedCount == 0);
        }

        private void GetDbContextWithUsers()
        {
            var department = new Department
            {
                Id = 1,
                Name = "TestDepartmentName",
                Image = "TestDepartmentImg.png",

            };

            dbContext.Departments.Add(department);

            var userDoctor = new User
            {
                Id = "TestUserId",
                Email = "testDoctor@vetclinic.com",
                UserName = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName"
            };

            dbContext.Users.Add(userDoctor);

            var doctor = new Doctor
            {
                Id = "testDoctorId",
                UserId = userDoctor.Id,
                Email = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName",
                PhoneNumber = "0111222333",
                ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/testDoctor.png",
                DepartmentId = 1,
                Description = "TestDescription"
            };

            dbContext.Doctors.Add(doctor);
            department.Doctors.Add(doctor);

            var userDoctor2 = new User
            {
                Id = "TestUserId2",
                Email = "testDoctor2@vetclinic.com",
                UserName = "testDoctor2@vetclinic.com",
                FullName = "TestDoctor FullName2"
            };

            dbContext.Users.Add(userDoctor2);

            var doctor2 = new Doctor
            {
                Id = "testDoctor2Id",
                UserId = userDoctor2.Id,
                Email = "testDoctor2@vetclinic.com",
                FullName = "TestDoctor FullName2",
                PhoneNumber = "0222333444",
                ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/testDoctor2.png",
                DepartmentId = 1,
                Description = "Some Description"
            };

            dbContext.Doctors.Add(doctor2);
            department.Doctors.Add(doctor2);

            var user = new User
            {
                Id = "TestUserId3",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName
            };

            dbContext.Clients.Add(client);

            var userAdmin = new User
            {
                Id = "TestAdmintUserId",
                Email = "testAdmin@vetclinic.com",
                UserName = "testAdmin@vetclinic.com",
                FullName = "TestAdminFullName"
            };

            dbContext.Users.Add(userAdmin);

            dbContext.SaveChanges();
        }
    }
}
