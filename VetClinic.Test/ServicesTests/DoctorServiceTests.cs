using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VetClinic.Core.Models.Doctors;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using DoctorService = VetClinic.Core.Services.DoctorService;

namespace VetClinic.Test.ServicesTests
{
    public class DoctorServiceTests
    {
        private VetClinicDbContext dbContext;
        private DoctorService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new DoctorService(UserManagerMock.Instance, dbContext);
        }

        [Test]
        public void AllShouldWorkCorrectAndReturnAllDoctorsInCorrectType()
        {
            GetDbContextWithDoctors();
            var result = service.All(null, null);
            var actualDoctorsCount = result.Doctors.Count();
            Assert.That(actualDoctorsCount, Is.EqualTo(2));
            Assert.That(result.GetType, Is.EqualTo(typeof(AllDoctorsViewModel)));
            Assert.That(result.Doctors.GetType(), Is.EqualTo(typeof(List<DoctorServiceModel>)));
        }

        [TestCase(null, "description")]
        [TestCase(null, "TestDepartmentName")]
        [TestCase("TestDepartmentName", null)]
        public void AllShouldWorkCorrectWithSearchByDepartment(string departmentName, string searchTerm)
        {
            GetDbContextWithDoctors();
            var result = service.All(departmentName, searchTerm);
            var actualServicesCount = result.Doctors.Count();
            Assert.That(actualServicesCount, Is.EqualTo(2));
            Assert.That(result.GetType, Is.EqualTo(typeof(AllDoctorsViewModel)));
            Assert.That(result.Doctors.GetType(), Is.EqualTo(typeof(List<DoctorServiceModel>)));
        }

        [TestCase("TestDepartmentName", "Some Description")]
        [TestCase("TestDepartmentName", "TestDoctorFullName")]
        [TestCase(null, "TestDescription")]
        [TestCase(null, "TestDoctorFullName")]
        public void AllShouldWorkCorrectWithSearch(string departmentName, string searchTerm)
        {
            GetDbContextWithDoctors();
            var result = service.All(departmentName, searchTerm);
            var actualServicesCount = result.Doctors.Count();
            Assert.That(actualServicesCount, Is.EqualTo(1));
            Assert.That(result.GetType, Is.EqualTo(typeof(AllDoctorsViewModel)));
            Assert.That(result.Doctors.GetType(), Is.EqualTo(typeof(List<DoctorServiceModel>)));
        }

        [Test]
        public void ByDepartmentShouldReturnAllDepartmentDoctorsInCorrectType()
        {
            GetDbContextWithDoctors();
            var availableDoctors = new AvailableDoctorsServiceModel
            {
                DepartmentId = 1,
                Doctors = new List<DoctorServiceModel>()
            };

            var result = service.ByDepartment(availableDoctors);
            var actualDoctorsCount = result.Doctors.Count();
            Assert.That(actualDoctorsCount, Is.EqualTo(2));
            Assert.That(result.GetType, Is.EqualTo(typeof(AvailableDoctorsServiceModel)));
            Assert.That(result.Doctors.GetType(), Is.EqualTo(typeof(List<DoctorServiceModel>)));
        }

        [Test]
        public void DetailsShouldReturnDoctorInCorrectType()
        {
            GetDbContextWithDoctors();
            var result = service.Details("testDoctorId");
            Assert.That(result.GetType, Is.EqualTo(typeof(DoctorDetailsServiceModel)));
        }

        [Test]
        public void DetailsShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithDoctors();
            var result = service.Details("NotExistingDoctorId");
            Assert.IsNull(result);
        }

        [Test]
        public void DoctorExistsShouldReturnTrueIfDoctorExists()
        {
            GetDbContextWithDoctors();
            var result = service.DoctorExists("TestDoctorFullName", "0111222333");
            Assert.That(result, Is.True);
        }

        [TestCase(null, null)]
        [TestCase("TestDoctorFullName", "9999123123")]
        [TestCase(null, "0111222333")]
        [TestCase(null, "9999123123")]
        [TestCase("NotExistingName", "0111222333")]
        [TestCase("NotExistingName", "9999123123")]
        [TestCase("NotExistingName", null)]
        public void DoctorExistsShouldReturnFalseIfDoctorNotExist(string fullName, string phoneNumber)
        {
            GetDbContextWithDoctors();
            var result = service.DoctorExists(fullName, phoneNumber);
            Assert.That(result, Is.False);
        }

        [Test]
        public void EditShouldReturnFalseWhenDoctorNotExist()
        {
            GetDbContextWithDoctors();
            var result = service.Edit(
                "NotExistingDoctorId",
                "NewTestDoctor FullName",
                "TestDoctorImg.png",
                "Test description",
                "testDoctor@vetclinic.com",
                "0111222333",
                1);
            Assert.That(result, Is.False);
        }

        [Test]
        public void EditShouldReturnTrueWhenDoctorExists()
        {
            GetDbContextWithDoctors();
            var result = service.Edit(
                "testDoctorId",
                "NewTestDoctor FullName",
                "TestDoctorImg.png",
                "Test description",
                "testDoctor@vetclinic.com",
                "0111222333",
                1);
            Assert.That(result, Is.True);
        }

        [Test]
        public void RegisterShouldWorkCorrectAndReturnUserId()
        {
            DoctorFormModel doctorModel = new DoctorFormModel
            {
                FullName = "Dr FullName",
                Email = "doctor@vetclinic.com",
                PhoneNumber = "1234555555",
                ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drDoctor.png",
                Description = "",
                DepartmentId = 1
            };
            var result = service.Register(doctorModel);
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void CreateShouldWorkCorrectAndReturnDoctorId()
        {
            GetDbContextWithDoctors();
            var result = service.Create(
                "Dr FullName",
                "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drDoctor.png",
                "1234555555",
                "doctor@vetclinic.com",
                null,
                1,
                "NewTestUserId");
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
            Assert.That(result, Is.Not.Null);
            Assert.That(dbContext.Doctors.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetDoctorShouldReturnDoctorInCorrectType()
        {
            GetDbContextWithDoctors();
            var result = service.GetDoctor("TestUserId");
            Assert.That(result.GetType, Is.EqualTo(typeof(DoctorServiceModel)));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("testDoctorId"));
            Assert.That(result.FullName, Is.EqualTo("TestDoctorFullName"));
            Assert.That(result.DepartmentId, Is.EqualTo(1));
        }

        [Test]
        public void GetDoctorShouldReturnNullWhenDoctorNotExists()
        {
            GetDbContextWithDoctors();
            var result = service.GetDoctor("NotExistingUserId");
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteShouldReturnFalseWhenDoctorNotExist()
        {
            //GetDbContextWithDoctors();
            GetDbContextWithAllEntities();
            var result = service.Delete("NotExistingDoctorId");
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteShouldReturnTrueWhenDoctorExists()
        {
            //GetDbContextWithDoctors();
            GetDbContextWithAllEntities();
            var result = service.Delete("testDoctorId");
            var actualDoctorsCount = dbContext.Doctors.Count();
            Assert.That(result, Is.True);
            Assert.That(actualDoctorsCount, Is.EqualTo(1));
        }

        private void GetDbContextWithDoctors()
        {
            var department = new Department
            {
                Id = 1,
                Name = "TestDepartmentName",
                Image = "TestDepartmentImg.png",
                Services = new List<Service>
                {
                    new Service
                    {
                        Id = 1,
                        Name = "TestService"
                    },
                    new Service
                    {
                        Id = 2,
                        Name = "TestService2"
                    }
                }
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
                Id = Guid.NewGuid().ToString(),
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

            dbContext.SaveChanges();
        }

        private void GetDbContextWithAllEntities()
        {
            var department = new Department
            {
                Id = 1,
                Name = "TestDepartmentName",
                Image = "TestDepartmentImg.png",
                Services = new List<Service>
                {
                    new Service
                    {
                        Id = 1,
                        Name = "TestService"
                    },
                    new Service
                    {
                        Id = 2,
                        Name = "TestService2"
                    }
                }
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
                Id = Guid.NewGuid().ToString(),
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
                Id = Guid.NewGuid().ToString(),
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
                FullName = user.FullName,
                Pets = new List<Pet>
                {
                    new Pet
                    {
                        Id = "NewTestPet1",
                        Name = "Pet1",
                        PetType = new PetType
                        {
                            Id = 1,
                            Name = "Dog"
                        },
                        Breed = "street",
                        DateOfBirth = DateTime.Now.Date.AddYears(-2),
                        Gender = Data.Enums.Gender.Male
                    },
                    new Pet
                    {
                        Id = "NewTestPet2",
                        Name = "Pet2",
                        PetType = new PetType
                        {
                            Id = 2,
                            Name = "Cat"
                        },
                        Breed = "Persian",
                        DateOfBirth = DateTime.Now.Date.AddMonths(-3),
                        Gender = Data.Enums.Gender.Female,
                    }
                },
            };

            dbContext.Clients.Add(client);

            var appointment = new Appointment
            {
                Id = "NewTestAppointmentId",
                Date = DateTime.Now.Date.AddDays(-5),
                Hour = "10:00",
                Doctor = doctor,
                Client = client,
                PetId = "NewTestPet2",
                ServiceId = 1
            };

            var prescription = new Prescription
            {
                Id = "NewTestPrescription",
                AppointmentId = "NewTestAppointmentId",
                CreatedOn = DateTime.Now,
                Description = "description",
                Doctor = doctor,
                PetId = "NewTestPet2"
            };

            dbContext.Appointments.Add(appointment);
            dbContext.Prescriptions.Add(prescription);

            doctor.Appointments.Add(appointment);
            doctor.Prescriptions.Add(prescription);

            dbContext.SaveChanges();
        }
    }
}
