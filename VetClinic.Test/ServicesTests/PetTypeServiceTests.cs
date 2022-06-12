using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using VetClinic.Core.Models.PetTypes;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;

namespace VetClinic.Test.ServicesTests
{
    public class PetTypeServiceTests
    {
        private VetClinicDbContext dbContext;
        private PetTypeService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new PetTypeService(dbContext);

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

            dbContext.PetTypes.AddRange(petTypes);
            dbContext.SaveChanges();
        }

        [Test]
        public void GetPetTypesShouldReturnAllPetTypesInCorrectType()
        {
            var result = service.GetPetTypes();
            var expectedPetTypesCount = dbContext.PetTypes.Count();
            Assert.That(result.Count(), Is.EqualTo(expectedPetTypesCount));
            Assert.That(result.GetType(), Is.EqualTo(typeof(List<PetTypeServiceModel>)));
        }
    }
}
