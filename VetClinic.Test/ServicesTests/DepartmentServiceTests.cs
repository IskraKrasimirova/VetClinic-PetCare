using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VetClinic.Core.Models.Departments;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using DoctorService = VetClinic.Core.Services.DoctorService;

namespace VetClinic.Test.ServicesTests
{
    public class DepartmentServiceTests
    {
        private VetClinicDbContext dbContext;
        private DepartmentService service;
        private DoctorService doctorService;
        private ServiceService serviceService;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            doctorService = new DoctorService(UserManagerMock.Instance, dbContext);
            serviceService = new ServiceService(dbContext);
            service = new DepartmentService(dbContext, doctorService, serviceService);
        }

        [Test]
        public void CreateShouldWorkCorrectAndReturnDepartmentId()
        {
            GetDbContextWithDepartments();
            var result = service.Create("DepartmentName", "departmentImg.png", "description");
            Assert.That(result.GetType, Is.EqualTo(typeof(int)));
            Assert.That(result == 3);
            Assert.That(dbContext.Departments.Count, Is.EqualTo(3));
        }

        [Test]
        public void DepartmentExistsShouldReturnTrueIfDepartmentNameExists()
        {
            GetDbContextWithDepartments();
            var result = service.DepartmentExists("TestDepartmentName");
            Assert.That(result, Is.True);
        }

        [Test]
        public void DepartmentExistsShouldReturnFalseIfDepartmentNameNotExist()
        {
            GetDbContextWithDepartments();
            var result = service.DepartmentExists("NotExistingDepartmentName");
            Assert.That(result, Is.False);
        }

        [Test]
        public void DepartmentExistsShouldReturnTrueIfDepartmentExists()
        {
            GetDbContextWithDepartments();
            var result = service.DepartmentExists(1);
            Assert.That(result, Is.True);
        }

        [Test]
        public void DepartmentExistsShouldReturnFalseIfDepartmentNotExist()
        {
            GetDbContextWithDepartments();
            var result = service.DepartmentExists(10);
            Assert.That(result, Is.False);
        }

        [Test]
        public void AllDepartmentsShouldReturnAllDepartmentsNames()
        {
            GetDbContextWithDepartments();
            var result = service.AllDepartments();
            Assert.That(result.GetType, Is.EqualTo(typeof(List<string>)));
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Has.Member("TestDepartmentName"));
        }

        [Test]
        public void AllDepartmentsShouldReturnEmptyCollectionWhenNoDepartmentsExist()
        {
            var result = service.AllDepartments();
            Assert.That(result.Count(), Is.EqualTo(0));
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllDepartmentsShouldReturnAllDepartmentsInCorrectType()
        {
            GetDbContextWithDepartments();
            var result = service.GetAllDepartments();
            Assert.That(result.GetType, Is.EqualTo(typeof(List<DepartmentListingViewModel>)));
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetAllDepartmentsShouldReturnNullWhenNoDepartmentsExist()
        {
            var result = service.GetAllDepartments();
            Assert.IsNull(result);
        }

        [Test]
        public void DetailsShouldReturnDepartmentInCorrectType()
        {
            GetDbContextWithDepartments();
            var result = service.Details(1);
            Assert.That(result.GetType, Is.EqualTo(typeof(DepartmentDetailsServiceModel)));
        }

        [Test]
        public void DetailsShouldReturnNullWhenDepartmentNotExist()
        {
            GetDbContextWithDepartments();
            var result = service.Details(10);
            Assert.IsNull(result);
        }

        [Test]
        public void EditShouldReturnFalseWhenDepartmentNotExist()
        {
            GetDbContextWithDepartments();
            var result = service.Edit(10, "testName", null, null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void EditShouldReturnTrueWhenDepartmentExists()
        {
            GetDbContextWithDepartments();
            var result = service.Edit(1, "NewTestDepartmentName", "TestDepartmentImage.png", "description");
            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteShouldReturnFalseWhenDepartmentNotExist()
        {
            GetDbContextWithDepartmentsAndDoctors();
            var result = service.Delete(10);
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteShouldReturnTrueWhenDepartmentExists()
        {
            GetDbContextWithDepartmentsAndDoctors();
            var result = service.Delete(1);
            var actualDepartmentsCount = dbContext.Departments.Count();
            Assert.That(result, Is.True);
            Assert.That(actualDepartmentsCount, Is.EqualTo(0));
        }


        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private void GetDbContextWithDepartments()
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
                        Name = "TestService"
                    },
                    new Service
                    {
                        Id = 2,
                        Name = "TestService2"
                    }
                }
            };

            dbContext.Departments.Add(testDepartment);

            var testDepartment2 = new Department
            {
                Id = 2,
                Name = "TestDepartmentName2",
                Image = "TestDepartmentImg2.png",
                Services = new List<Service>
                {
                    new Service
                    {
                        Id = 3,
                        Name = "TestService3"
                    },
                    new Service
                    {
                        Id = 4,
                        Name = "TestService4"
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
