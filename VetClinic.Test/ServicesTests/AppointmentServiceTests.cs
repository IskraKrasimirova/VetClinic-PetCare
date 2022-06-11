using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VetClinic.Core.Models.Appointments;
using VetClinic.Core.Models.Services;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Test.ServicesTests
{
    public class AppointmentServiceTests
    {
        private VetClinicDbContext dbContext;
        private AppointmentService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new AppointmentService(dbContext);
        }

        [Test]
        public void GetDoctorScheduleShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetDoctorSchedule("testDoctorId");
            var expectedDoctorFullName = dbContext
                .Doctors
                .First(d => d.Id == "testDoctorId")
                .FullName;
            Assert.That(result.GetType, Is.EqualTo(typeof(BookAppointmentServiceModel)));
            Assert.That(result.DoctorFullName, Is.EqualTo(expectedDoctorFullName));
            Assert.That(result.WorkHours.GetType(), Is.EqualTo(typeof(List<string>)));
        }

        [TestCase(null)]
        [TestCase("NotExistingDoctorId")]
        public void GetDoctorScheduleShouldReturnNullWhenDoctorNotExist(string doctorId)
        {
            GetDbContextWithAllEntities();
            var result = service.GetDoctorSchedule(doctorId);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void TryToParseDateShouldWorkCorrectAndReturnCorrectDateTimeFormat()
        {
            GetDbContextWithAllEntities();
            var result = service.TryToParseDate("15-06-2022", "09:00");
            Assert.That(result.GetType, Is.EqualTo(typeof(DateTime)));
            Assert.That(result.ToString("dd-MM-yyyy hh:mm"), Is.EqualTo("15-06-2022 09:00"));
        }

        [TestCase("15.06.2022", "9")]
        [TestCase("2022-06-15", "09:00")]
        public void TryToParseDateShouldReturnMinDateTimeWhenNotParsed(string dateAsString, string hourAsString)
        {
            GetDbContextWithAllEntities();
            var result = service.TryToParseDate(dateAsString, hourAsString);
            Assert.That(result.GetType, Is.EqualTo(typeof(DateTime)));
            Assert.That(result, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void CheckDoctorAvailableHoursShouldWorkCorrectAndReturnNullWhenAppointmentIsFree()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            var result = service.CheckDoctorAvailableHours(appointmentDate, "09:00", "testDoctorId");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void CheckDoctorAvailableHoursShouldWorkCorrectAndReturnCorrectMessageWhenAppointmentDateAndHourNotFree()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            var result = service.CheckDoctorAvailableHours(appointmentDate, "10:00", "testDoctorId");
            var availableHours = "09:00 11:00 12:00 13:00 14:00 15:00 16:00 17:00 18:00";
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
            Assert.That(result, Is.EqualTo($"The available hours for the date {appointmentDate.ToString(DateFormat)} are: {availableHours}"));
        }

        [Test]
        public void CheckDoctorAvailableHoursShouldWorkCorrectAndReturnCorrectMessageWhenDoctorHasNoFreeHours()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            var result = service.CheckDoctorAvailableHours(appointmentDate, "09:00", "testDoctor2Id");
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
            Assert.That(result, Is.EqualTo("There are no available hours for that day."));
        }

        [Test]
        public void CheckDoctorAvailableHoursShouldWorkCorrectAndReturnCorrectMessageWhenCanNotParseHour()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            var result = service.CheckDoctorAvailableHours(appointmentDate, "9", "testDoctorId");
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
            Assert.That(result, Is.EqualTo("Oops...Something went wrong!"));
        }

        [Test]
        public void AddNewAppointmentShouldWorkCorrect()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);
            service.AddNewAppointment("testClientId", "testDoctorId", 1, "NewTestPet1", appointmentDate, "09:00");
            var appointmentsCount = dbContext.Appointments.Count();
            Assert.That(appointmentsCount == 13);
        }

        [Test]
        public void AllServicesShouldWorkCorrectAndReturnDoctorServicesInCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.AllServices("testDoctorId");
            Assert.That(result.GetType, Is.EqualTo(typeof(List<ServiceViewModel>)));
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void AllServicesShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.AllServices("NotExistingDoctorId");
            Assert.IsNull(result);
        }

        [Test]
        public void GetUpcomingAppointmentsShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetUpcomingAppointments("testClientId");
            Assert.That(result.GetType, Is.EqualTo(typeof(List<UpcomingAppointmentServiceModel>)));
            Assert.That(result.Count, Is.EqualTo(11));
        }

        [Test]
        public void GetUpcomingAppointmentsShouldReturnNullWhenClientNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetUpcomingAppointments("NotExistingClientId");
            Assert.IsNull(result);
        }

        [Test]
        public void GetPastAppointmentsShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetPastAppointments("testClientId");
            Assert.That(result.GetType, Is.EqualTo(typeof(List<PastAppointmentServiceModel>)));
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetPastAppointmentsShouldReturnNullWhenClientNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetPastAppointments("NotExistingClientId");
            Assert.IsNull(result);
        }

        [Test]
        public void GetAppointmentForCancelShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetAppointmentForCancel("NewTestAppointmentId");
            Assert.That(result.GetType, Is.EqualTo(typeof(CancelAppointmentServiceModel)));
        }

        [TestCase("NotExistingAppointmentId")]
        [TestCase("TestAppointmentId2")]
        public void GetAppointmentForCancelShouldReturnNullWhenAppointmentNotExistOrIsInPast(string appointmentId)
        {
            GetDbContextWithAllEntities();
            var result = service.GetAppointmentForCancel(appointmentId);
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteShouldReturnFalseWhenAppointmentNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.Delete("NotExistingAppointmentId");
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteShouldReturnTrueWhenAppointmentExists()
        {
            GetDbContextWithAllEntities();
            var result = service.Delete("NewTestAppointmentId");
            var actualAppointmentsCount = dbContext.Appointments.Count();
            Assert.That(result, Is.True);
            Assert.That(actualAppointmentsCount, Is.EqualTo(11));
        }

        [Test]
        public void GetDoctorUpcomingAppointmentsShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetDoctorUpcomingAppointments("TestUserId");
            Assert.That(result.GetType, Is.EqualTo(typeof(List<UpcomingAppointmentServiceModel>)));
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetDoctorUpcomingAppointmentsShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetDoctorUpcomingAppointments("NotExistingUserId");
            Assert.IsNull(result);
        }

        [Test]
        public void GetDoctorPastAppointmentsShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetDoctorPastAppointments("TestUserId");
            Assert.That(result.GetType, Is.EqualTo(typeof(List<PastAppointmentServiceModel>)));
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetDoctorPastAppointmentsShouldReturnNullWhenDoctorNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetDoctorPastAppointments("NotExistingUserId");
            Assert.IsNull(result);
        }

        [Test]
        public void GetPastAppointmentShouldWorkCorrectAndReturnCorrectType()
        {
            GetDbContextWithAllEntities();
            var result = service.GetPastAppointment("TestAppointmentId2");
            Assert.That(result.GetType, Is.EqualTo(typeof(PastAppointmentServiceModel)));
            Assert.That(result.DoctorId, Is.EqualTo("testDoctorId"));
            Assert.That(result.DoctorFullName, Is.EqualTo("TestDoctorFullName"));
            Assert.That(result.ClientId, Is.EqualTo("testClientId"));
            Assert.That(result.PetId, Is.EqualTo("NewTestPet2"));
            Assert.That(result.PetName, Is.EqualTo("Pet2"));
            Assert.That(result.ServiceId, Is.EqualTo(1));
            Assert.That(result.ServiceName, Is.EqualTo("TestService"));
            Assert.That(result.Date, Is.EqualTo(DateTime.Now.Date.AddDays(-2)));

        }

        [Test]
        public void GetPastAppointmentShouldReturnNullWhenAppointmentNotExist()
        {
            GetDbContextWithAllEntities();
            var result = service.GetPastAppointment("NotExistingAppointmentId");
            Assert.IsNull(result);
        }

        [Test]
        public void CheckPetAppointmentsAtTheSameDateAndHourShouldReturnFalseWhenAppointmentNotExist()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(2).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);

            var result = service.CheckPetAppointmentsAtTheSameDateAndHour(appointmentDate, "10:00", "NewTestPet2", "testClientId");
            Assert.That(result, Is.False);
        }

        [Test]
        public void CheckPetAppointmentsAtTheSameDateAndHourShouldReturnTrueWhenAppointmentExists()
        {
            GetDbContextWithAllEntities();
            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);

            var result = service.CheckPetAppointmentsAtTheSameDateAndHour(appointmentDate, "10:00", "NewTestPet2", "testClientId");
            Assert.That(result, Is.True);
        }

        private void GetDbContextWithAllEntities()
        {
            var department = new Department
            {
                Id = 1,
                Name = "TestDepartmentName",
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

            var dateAsString = DateTime.Now.Date.AddDays(5).ToString(DateFormat);
            DateTime.TryParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);

            var appointment = new Appointment
            {
                Id = "NewTestAppointmentId",
                Date = appointmentDate,
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

            var appointment2 = new Appointment
            {
                Id = "TestAppointmentId2",
                Date = DateTime.Now.Date.AddDays(-2),
                Hour = "10:00",
                Doctor = doctor,
                Client = client,
                PetId = "NewTestPet2",
                ServiceId = 1
            };

            dbContext.Appointments.Add(appointment2);
            doctor.Appointments.Add(appointment2);

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = "NewTestAppointmentId1",
                    Date = appointmentDate,
                    Hour = "09:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet2",
                    ServiceId = 2
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId2",
                    Date = appointmentDate,
                    Hour = "10:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet2",
                    ServiceId = 2
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId3",
                    Date = appointmentDate,
                    Hour = "11:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 2
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId4",
                    Date = appointmentDate,
                    Hour = "12:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 2
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId5",
                    Date = appointmentDate,
                    Hour = "13:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 2
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId6",
                    Date = appointmentDate,
                    Hour = "14:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 1
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId7",
                    Date = appointmentDate,
                    Hour = "15:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 1
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId8",
                    Date = appointmentDate,
                    Hour = "16:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 1
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId9",
                    Date = appointmentDate,
                    Hour = "17:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 1
                },
                new Appointment
                {
                    Id = "NewTestAppointmentId10",
                    Date = appointmentDate,
                    Hour = "18:00",
                    Doctor = doctor2,
                    Client = client,
                    PetId = "NewTestPet1",
                    ServiceId = 1
                },
            };

            dbContext.Appointments.AddRange(appointments);

            dbContext.SaveChanges();
        }
    }
}
