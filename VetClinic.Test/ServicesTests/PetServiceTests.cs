using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Pets;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using PetService = VetClinic.Core.Services.PetService;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

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

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName ="TestName"
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
                    DateOfBirth = DateTime.Now.Date.AddDays(-2),
                    Gender = Data.Enums.Gender.Male,
                    ClientId = client.Id
                }
            };

            foreach (var pet in pets)
            {
                client.Pets.Add(pet);
            }

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

            data.PetTypes.AddRange(petTypes);
            data.Pets.AddRange(pets);
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
            var result = service.Edit("NewTestPet1","Pet1", DateTime.UtcNow.AddYears(-2), "basset", "Male", null, 1);
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
    }
}

