using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using VetClinic.Core.Models.Prescriptions;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using static VetClinic.Common.GlobalConstants.FormattingConstants;


namespace VetClinic.Test.ServicesTests
{
    public class PrescriptionServiceTests
    {
        private VetClinicDbContext dbContext;
        private PrescriptionService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new PrescriptionService(dbContext);
        }

        [Test]
        public void CreateShouldWorkCorrectWhenAppointmentExists()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            service.Create(appointmentDate, "Test description", "NewTestAppointmentId2");
            var prescriptionsCount = dbContext.Prescriptions.Count();
            Assert.That(prescriptionsCount, Is.EqualTo(4));
        }

        [Test]
        public void CreateShouldThrowExceptionWhenAppointmentNotExist()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            Assert.That(() => service.Create(appointmentDate, "Test description", "NotExistingAppointmentId"), Throws.Exception);
        }

        [Test]
        public void CreateShouldNotCreatePrescriptionWhenPrescriptionAlreadyExists()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            service.Create(appointmentDate, "description", "NewTestAppointmentId1");
            var prescriptionsCount = dbContext.Prescriptions.Count();
            Assert.That(prescriptionsCount, Is.EqualTo(3));
        }

        [Test]
        public void DetailsShouldReturnPrescriptionInCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.Details("TestPrescriptionId");
            Assert.That(result.GetType, Is.EqualTo(typeof(PrescriptionServiceModel)));
        }

        [Test]
        public void DetailsShouldReturnNullWhenPrescriptionNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.Details("NotExistingPrescription");
            Assert.IsNull(result);
        }

        [Test]
        public void GetMineShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetMine("testDoctor2Id");
            var expectedDoctorPrescriptionsCount = dbContext.Prescriptions
                .Where(p => p.DoctorId == "testDoctor2Id")
                .Count();
            Assert.That(result.GetType, Is.EqualTo(typeof(List<PrescriptionServiceModel>)));
            Assert.That(result.Count, Is.EqualTo(expectedDoctorPrescriptionsCount));
        }

        [Test]
        public void GetMineShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetMine("NotExistingDoctorId");
            Assert.IsNull(result);
        }

        [Test]
        public void GetPrescriptionsByPetShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetPrescriptionsByPet("NewTestPet2");
            var expectedPetPrescriptionsCount = dbContext.Prescriptions
                .Where(p => p.PetId == "NewTestPet2")
                .Count();
            Assert.That(result.GetType, Is.EqualTo(typeof(List<PrescriptionServiceModel>)));
            Assert.That(result.Count, Is.EqualTo(expectedPetPrescriptionsCount));
        }

        [Test]
        public void GetPrescriptionsByPetShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetPrescriptionsByPet("NotExistingPetId");
            Assert.IsNull(result);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private void GetDbContextWithAllEntities()
        {
            var department = new Department
            {
                Id = 1,
                Name = "TestDepartmentName",
                Image = "DepartmentImg.png",
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

            var dateAsString = DateTime.Now.Date.AddDays(-5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);

            var appointments = new List<Appointment>
            { 
                new Appointment
                {
                    Id = "TestAppointmentId",
                    Date = appointmentDate,
                    Hour = "10:00",
                    Doctor = doctor,
                    Client = client,
                    PetId = "NewTestPet2",
                    ServiceId = 1
                },
                new Appointment
                {
                    Id = "TestAppointmentId2",
                    Date = DateTime.Now.Date.AddDays(-2),
                    Hour = "10:00",
                    Doctor = doctor,
                    Client = client,
                    PetId = "NewTestPet2",
                    ServiceId = 2
                }
            };

            var prescriptions = new List<Prescription>
            {
                new Prescription
                {
                    Id = "TestPrescriptionId",
                    AppointmentId = "TestAppointmentId",
                    CreatedOn = DateTime.Now,
                    Description = "description",
                    Doctor = doctor,
                    PetId = "NewTestPet2"
                },
                new Prescription
                {
                    Id = "TestPrescriptionId2",
                    AppointmentId = "TestAppointmentId2",
                    CreatedOn = DateTime.Now,
                    Description = "some description",
                    Doctor = doctor,
                    PetId = "NewTestPet2"
                },
            };

            dbContext.Appointments.AddRange(appointments);
            dbContext.Prescriptions.AddRange(prescriptions);

            doctor.Appointments = appointments;
            doctor.Prescriptions = prescriptions;

            var appointment = new Appointment
            {
                Id = "NewTestAppointmentId1",
                Date = appointmentDate,
                Hour = "11:00",
                Doctor = doctor2,
                Client = client,
                PetId = "NewTestPet1",
                ServiceId = 1
            };

            var appointment2 = new Appointment
            {
                Id = "NewTestAppointmentId2",
                Date = appointmentDate,
                Hour = "12:00",
                Doctor = doctor2,
                Client = client,
                PetId = "NewTestPet1",
                ServiceId = 1
            };

            var prescription = new Prescription
            {
                Id = "NewTestPrescriptionId",
                AppointmentId = "NewTestAppointmentId1",
                CreatedOn = DateTime.Now,
                Description = "description",
                Doctor = doctor2,
                PetId = "NewTestPet1"
            };

            dbContext.Appointments.Add(appointment);
            dbContext.Appointments.Add(appointment2);
            dbContext.Prescriptions.Add(prescription);
            doctor2.Appointments.Add(appointment);
            doctor2.Prescriptions.Add(prescription);
            appointment.PrescriptionId = prescription.Id;

            dbContext.SaveChanges();
        }
    }
}
