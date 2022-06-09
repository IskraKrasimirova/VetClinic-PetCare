using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VetClinic.Core.Models.Services;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;

namespace VetClinic.Test.ServicesTests
{
    public class ServiceServiceTests
    {
        private VetClinicDbContext dbContext;
        private ServiceService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new ServiceService(dbContext);
        }

        [Test]
        public void AllShouldWorkCorrectAndReturnAllServicesInCorrectType()
        {
            GetDbContextWithServices();
            var result = service.All(null, null);
            var actualServicesCount = result.Services.Count();
            Assert.That(actualServicesCount, Is.EqualTo(4));
            Assert.That(result.GetType, Is.EqualTo(typeof(AllServicesViewModel)));
            Assert.That(result.Services.GetType(), Is.EqualTo(typeof(List<ServiceViewModel>)));
        }

        [TestCase(null, "TestDepartmentName")]
        [TestCase("TestDepartmentName", null)]
        public void AllShouldWorkCorrectWithSearchByDepartment(string departmentName, string searchTerm)
        {
            GetDbContextWithServices();
            var result = service.All(departmentName, searchTerm);
            var actualServicesCount = result.Services.Count();
            Assert.That(actualServicesCount, Is.EqualTo(2));
            Assert.That(result.GetType, Is.EqualTo(typeof(AllServicesViewModel)));
            Assert.That(result.Services.GetType(), Is.EqualTo(typeof(List<ServiceViewModel>)));
        }

        [TestCase("Test Department Name2", "TestDescription")]
        [TestCase("TestDepartmentName", "Test Service2")]
        [TestCase("TestDepartmentName", "TestService")]
        [TestCase(null, "TestDescription")]
        [TestCase(null, "TestService")]
        public void AllShouldWorkCorrectWithSearch(string departmentName, string searchTerm)
        {
            GetDbContextWithServices();
            var result = service.All(departmentName, searchTerm);
            var actualServicesCount = result.Services.Count();
            Assert.That(actualServicesCount, Is.EqualTo(1));
            Assert.That(result.GetType, Is.EqualTo(typeof(AllServicesViewModel)));
            Assert.That(result.Services.GetType(), Is.EqualTo(typeof(List<ServiceViewModel>)));
        }

        [Test]
        public void ByDepartmentShouldReturnAllDepartmentServicesInCorrectType()
        {
            GetDbContextWithServices();
            var availableServices = new AvailableServicesViewModel
            {
                DepartmentId = 1,
                Services = new List<ServiceViewModel>()
            };

            var result = service.ByDepartment(availableServices);
            var actualServicesCount = result.Services.Count();
            Assert.That(actualServicesCount, Is.EqualTo(2));
            Assert.That(result.GetType, Is.EqualTo(typeof(AvailableServicesViewModel)));
            Assert.That(result.Services.GetType(), Is.EqualTo(typeof(List<ServiceViewModel>)));
        }

        [Test]
        public void ServiceExistsShouldReturnTrueIfServiceNameExists()
        {
            GetDbContextWithServices();
            var result = service.ServiceExists("Test Service2");
            Assert.That(result, Is.True);
        }

        [Test]
        public void ServiceExistsShouldReturnFalseIfServiceNameNotExist()
        {
            GetDbContextWithServices();
            var result = service.ServiceExists("NotExistingServiceName");
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateShouldWorkCorrectAndReturnServiceId()
        {
            GetDbContextWithServices();
            var result = service.Create("New Test Service", null, 10.0M, 1);
            Assert.That(result.GetType, Is.EqualTo(typeof(int)));
            Assert.That(result == 5);
            Assert.That(dbContext.Services.Count, Is.EqualTo(5));
        }

        [Test]
        public void DetailsShouldReturnServiceInCorrectType()
        {
            GetDbContextWithServices();
            var result = service.Details(1);
            Assert.That(result.GetType, Is.EqualTo(typeof(ServiceViewModel)));
        }

        [Test]
        public void DetailsShouldReturnNullWhenServiceNotExist()
        {
            GetDbContextWithServices();
            var result = service.Details(10);
            Assert.IsNull(result);
        }

        [Test]
        public void EditShouldReturnFalseWhenServiceNotExist()
        {
            GetDbContextWithServices();
            var result = service.Edit(100, "New TestService", "Test description", 10.50M, 1);
            Assert.That(result, Is.False);
        }

        [Test]
        public void EditShouldReturnTrueWhenServiceExists()
        {
            GetDbContextWithServices();
            var result = service.Edit(1, "New TestService", "Test description", 10.50M, 1);
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteShouldReturnFalseWhenServiceNotExist()
        {
            //GetDbContextWithServices();
            GetDbContextWithDepartmentsAndDoctors();
            var result = service.Delete(10);
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteShouldReturnTrueWhenServiceExists()
        {
            //GetDbContextWithServices();
            GetDbContextWithDepartmentsAndDoctors();
            var result = service.Delete(1);
            var actualServicesCount = dbContext.Services.Count();
            Assert.That(result, Is.True);
            Assert.That(actualServicesCount, Is.EqualTo(1));  
        }

        private void GetDbContextWithServices()
        {
            var testDepartment = new Department
            {
                Id = 1,
                Name = "TestDepartmentName",
                Image = "TestDepartmentImg.png",
                Services = new List<Service>
                {
                    new Service
                    {
                        Id = 1,
                        Name = "TestService",
                        Description = "Some test description"
                    },
                    new Service
                    {
                        Id = 2,
                        Name = "Test Service2",
                        Description = "Another test description"
                    }
                }
            };

            dbContext.Departments.Add(testDepartment);

            var testDepartment2 = new Department
            {
                Id = 2,
                Name = "Test Department Name2",
                Image = "TestDepartmentImg2.png",
                Services = new List<Service>
                {
                    new Service
                    {
                        Id = 3,
                        Name = "Test Service3",
                        Description = "TestDescription"
                    },
                    new Service
                    {
                        Id = 4,
                        Name = "Test Service4"
                    }
                }
            };

            dbContext.Departments.Add(testDepartment2);

            dbContext.SaveChanges();
        }

        private void GetDbContextWithDepartmentsAndDoctors()
        {
            var department = new Department
            {
                Id = 1,
                Name = "TestDepartment",
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
                Id = Guid.NewGuid().ToString(),
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
                DepartmentId = 1
            };

            dbContext.Doctors.Add(doctor);
            department.Doctors.Add(doctor);

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
