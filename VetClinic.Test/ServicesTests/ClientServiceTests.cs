using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;

namespace VetClinic.Test.ServicesTests
{
    public class ClientServiceTests
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
                .AddSingleton<IClientService, ClientService>()
                .BuildServiceProvider();

            var data = serviceProvider.GetRequiredService<VetClinicDbContext>();

            var user = new User
            {
                Id = "testUserId",
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

            var user2 = new User
            {
                Id = "testUser2Id",
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

            data.SaveChanges();
        }

        [Test]
        public void GetClientIdShouldReturnClientId()
        {
            var service = serviceProvider.GetService<IClientService>();
            var result = service.GetClientId("testUserId");
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetClientIdShouldReturnNullWhenUserNotExist()
        {
            var service = serviceProvider.GetService<IClientService>();
            var result = service.GetClientId("NotExistingUserId");
            Assert.IsTrue(result == null);
        }
    }
}
