﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.PetTypes;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;

namespace VetClinic.Test.ServicesTests
{
    public class PetTypeServiceTests
    {
        //private ServiceProvider serviceProvider;
        //private InMemoryDbContext dbContext;

        //[SetUp]
        //public void Setup()
        //{
        //    dbContext = new InMemoryDbContext();
        //    var serviceCollection = new ServiceCollection();

        //    serviceProvider = serviceCollection
        //        .AddSingleton(sp => dbContext.CreateContext())
        //        .AddSingleton<IPetTypeService, PetTypeService>()
        //        .BuildServiceProvider();

        //    var petTypes = new List<PetType>
        //    {
        //        new PetType
        //        {
        //            Id = 1,
        //            Name = "Dog"
        //        },
        //        new PetType
        //        {
        //            Id = 2,
        //            Name = "Cat"
        //        }
        //    };

        //    //var data = serviceProvider.GetService<InMemoryDbContext>();
        //    //serviceProvider.GetServices<InMemoryDbContext>();
        //    //dbContext.CreateContext().PetTypes.AddRange(petTypes);

        //    var data = dbContext.CreateContext();
        //    data.PetTypes.AddRange(petTypes);
        //    data.SaveChanges();
        //}

        //[Test]
        //public void GetPetTypesShouldReturnAll()
        //{
        //    var service = serviceProvider.GetService<PetTypeService>();
        //    var result = service.GetPetTypes();
        //    Assert.True(result.Count() == 2);
        //    //Assert.That(result.Count(), Is.EqualTo(2));
        //    //Assert.That(result.GetType(), Is.EqualTo(typeof(PetTypeServiceModel))); 
        //}

        private readonly VetClinicDbContext dbContext;
        private readonly PetTypeService service;

        public PetTypeServiceTests()
        {
            dbContext = DatabaseMock.Instance;
            service = new PetTypeService(dbContext);
        }

        [SetUp]
        public void Setup()
        {
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

        [TearDown]
        public void Dispose()
        {
            dbContext.Dispose();
        }

        [Test]
        public void GetPetTypesShouldReturnAll()
        {
            var result = service.GetPetTypes();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.GetType(), Is.EqualTo(typeof(List<PetTypeServiceModel>)));
        }
    }
}
