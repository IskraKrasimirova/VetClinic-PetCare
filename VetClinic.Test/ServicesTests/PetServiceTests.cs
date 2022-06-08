using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Pets;
using VetClinic.Data;
using VetClinic.Data.Models;
using PetService = VetClinic.Core.Services.PetService;

namespace VetClinic.Test.ServicesTests
{
    public class PetServiceTests
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext; 

        [SetUp]
        public void Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IPetService, PetService>()
                .BuildServiceProvider();

            var data = serviceProvider.GetRequiredService<VetClinicDbContext>();
            //var userManagerMock = UserManagerMock.Instance;

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
                    }
                }
            };

            data.Departments.Add(department);

            var userDoctor = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "testDoctor@vetclinic.com",
                UserName = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName"
            };

            //userManagerMock.CreateAsync(userDoctor, "testPassword");
            //userManagerMock.AddToRoleAsync(userDoctor, "Doctor");
            data.Users.Add(userDoctor);

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

            data.Doctors.Add(doctor);
            department.Doctors.Add(doctor);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            data.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName
            };

            data.Clients.Add(client);

            var petTypes = new List<PetType>
            {
                new PetType
                {
                    Id = 1,
                    Name = "Dog"
                },
                new PetType
                {
                    Id = 2,
                    Name = "Cat"
                }
            };

            var pets = new List<Pet>
            {
                new Pet
                {
                    Id = "NewTestPet1",
                    Name = "Pet1",
                    PetTypeId = 1,
                    Breed = "street",
                    DateOfBirth = DateTime.Now.Date,
                    Gender = Data.Enums.Gender.Male,
                    ClientId = client.Id
                },
                new Pet
                {
                    Id = "NewTestPet2",
                    Name = "Pet2",
                    PetTypeId = 2,
                    Breed = "Persian",
                    DateOfBirth = DateTime.Now.Date.AddMonths(-3),
                    Gender = Data.Enums.Gender.Male,
                    ClientId = client.Id,
                }
            };

            foreach (var pet in pets)
            {
                client.Pets.Add(pet);
            }

            var testPet2 = pets.FirstOrDefault(p => p.Id == "NewTestPet2");

            var petDoctor = new PetDoctor
            {
                PetId = "NewTestPet2",
                DoctorId = "testDoctorId"
            };

            testPet2.PetDoctors.Add(petDoctor);
            doctor.PetDoctors.Add(petDoctor);

            data.PetTypes.AddRange(petTypes);
            data.Pets.AddRange(pets);
            data.PetDoctors.Add(petDoctor);

            var user2 = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test2@test.com",
                UserName = "test2@test.com",
                PhoneNumber = "0999777111",
                FullName = "TestName2"
            };

            data.Users.Add(user2);

            var client2 = new Client
            {
                Id = "testClient2Id",
                UserId = user2.Id,
                FullName = user2.FullName
            };

            data.Clients.Add(client2);

            var appointment = new Appointment
            {
                Id = "NewTestAppointmentId",
                Date = DateTime.Now.Date.AddDays(-5),
                Hour = "10:00",
                Doctor = doctor,
                Client = client,
                Pet = testPet2,
                ServiceId = 1
            };

            var prescription = new Prescription
            {
                Id = "NewTestPrescription",
                AppointmentId = appointment.Id,
                CreatedOn = DateTime.Now,
                Description = "description",
                Doctor = doctor,
                Pet = testPet2
            };

            data.Appointments.Add(appointment);
            data.Prescriptions.Add(prescription);

            doctor.Appointments.Add(appointment);
            doctor.Prescriptions.Add(prescription);
            testPet2.Appointments.Add(appointment);
            testPet2.Prescriptions.Add(prescription);

            data.SaveChanges();
        }

        [Test]
        public void AllShouldReturnAll()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.All(null, null);
            var expectedPets = result.Pets.Count();
            Assert.That(expectedPets, Is.EqualTo(2));
            Assert.That(result.Pets.GetType(), Is.EqualTo(typeof(List<PetListingViewModel>)));
        }

        [TestCase("Cat", "Pet2")]
        [TestCase("Cat", "Persian")]
        [TestCase(null, "Persian")]
        [TestCase(null, "Pet2")]
        [TestCase("Cat", null)]
        public void AllShouldWorkCorrectWithSearch(string petTypeName, string searchTerm)
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.All(petTypeName, searchTerm);
            var expectedPetsCount = result.Pets.Count();
            Assert.That(expectedPetsCount, Is.EqualTo(1));
            Assert.That(result.Pets.GetType(), Is.EqualTo(typeof(List<PetListingViewModel>)));
        }

        [Test]
        public void AllPetTypesShouldReturnOrderedAllPetTypeNames()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.AllPetTypes();
            var expectedPetsCount = result.Count();
            Assert.That(expectedPetsCount, Is.EqualTo(2));
            Assert.That(result.GetType(), Is.EqualTo(typeof(List<string>)));
            Assert.That(result, Has.Member("Dog"));
            Assert.That(result.First, Is.EqualTo("Cat"));
        }

        [Test]
        public void ByClientShouldReturnAllClientPets()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.ByClient("testClientId");
            var expectedPetsCount = result.Count();
            Assert.That(expectedPetsCount, Is.EqualTo(2));
            Assert.That(result.GetType(), Is.EqualTo(typeof(List<PetListingViewModel>)));
        }

        [Test]
        public void ByClientShouldReturnNoPetsIfClientNotExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.ByClient(Guid.NewGuid().ToString());
            var expectedPetsCount = result.Count();
            Assert.That(expectedPetsCount, Is.EqualTo(0));
        }

        [Test]
        public void ByClientShouldReturnNoPetsIfClientHasNoPets()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.ByClient("testClient2Id");
            var expectedPetsCount = result.Count();
            Assert.That(expectedPetsCount, Is.EqualTo(0));
        }

        [Test]
        public void PetTypeExistsReturnsTrueIfExists()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.PetTypeExists(1);
            Assert.That(result, Is.True);
        }

        [Test]
        public void PetTypeExistsReturnsFalseIfNotExists()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.PetTypeExists(10);
            Assert.That(result, Is.False);
        }

        [Test]
        public void AddPetShouldWorkCorrectAndReturnsPetId()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.AddPet("pet3", DateTime.UtcNow.AddYears(-2), "basset", "Female", null, 1, "testClientId");
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void EditShouldReturnTrueWhenPetDataAreCorrect()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.Edit("NewTestPet1", "Pet1", DateTime.UtcNow.AddYears(-2), "basset", "Male", null, 1);
            Assert.That(result, Is.True);
        }

        [Test]
        public void EditShouldReturnFalseWhenPetNotExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.Edit("NotExistingPetId", "Pet1", DateTime.UtcNow.AddYears(-2), "basset", "Male", null, 1);
            Assert.That(result, Is.False);
        }

        [Test]
        public void DetailsShouldReturnCorrectPetModel()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.Details("NewTestPet1");
            Assert.That(result.GetType, Is.EqualTo(typeof(PetDetailsServiceModel)));
        }

        [Test]
        public void DetailsShouldReturnNullWhenPetNotExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.Details("NotExistingPetId");
            Assert.IsTrue(result == null);
        }

        [Test]
        public void GetPetForDeleteShouldReturnCorrectPetModel()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.GetPetForDelete("NewTestPet1");
            Assert.That(result.GetType, Is.EqualTo(typeof(PetDeleteServiceModel)));
        }

        [Test]
        public void GetPetForDeleteShouldReturnNullWhenPetNotExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.GetPetForDelete("NotExistingPetId");
            Assert.IsTrue(result == null);
        }

        [Test]
        public void DeleteShouldReturnTrueWhenPetAndOwnerExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.Delete("NewTestPet2", "testClientId");
            Assert.That(result, Is.True);
        }

        [TestCase("NewTestPet2", "testClient2Id")]
        [TestCase("NewTestPet2", "NotExistingClientId")]
        [TestCase("NotExistingPetId", "testClientId")]
        public void DeleteShouldReturnFalseWhenPetOrOwnerNotExist(string petId, string clientId)
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.Delete(petId, clientId);
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsByOwnerShouldReturnTrueWhenPetAndOwnerExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.IsByOwner("NewTestPet2", "testClientId");
            Assert.That(result, Is.True);
        }

        [TestCase("NewTestPet2", "testClient2Id")]
        [TestCase("NewTestPet2", "NotExistingClientId")]
        [TestCase("NotExistingPetId", "testClientId")]
        public void IsByOwnerShouldReturnFalseWhenPetOrOwnerNotExist(string petId, string clientId)
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.IsByOwner(petId, clientId);
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetPetShouldReturnCorrectPetModel()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.GetPet("NewTestPet1");
            Assert.That(result.GetType, Is.EqualTo(typeof(PetServiceModel)));
        }

        [Test]
        public void GetPetShouldReturnNullWhenPetNotExist()
        {
            var service = serviceProvider.GetService<IPetService>();
            var result = service.GetPet("NotExistingPetId");
            Assert.IsTrue(result == null);
        }
    }
}

