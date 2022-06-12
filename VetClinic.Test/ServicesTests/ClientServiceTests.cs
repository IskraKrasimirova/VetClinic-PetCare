using NUnit.Framework;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;

namespace VetClinic.Test.ServicesTests
{
    public class ClientServiceTests
    {
        private VetClinicDbContext dbContext;
        private ClientService service;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new ClientService(dbContext);

            var user = new User
            {
                Id = "testUserId",
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
                FullName = user.FullName
            };

            dbContext.Clients.Add(client);

            var user2 = new User
            {
                Id = "testUser2Id",
                Email = "test2@test.com",
                UserName = "test2@test.com",
                PhoneNumber = "0999777111",
                FullName = "TestName2"
            };

            dbContext.Users.Add(user2);

            var client2 = new Client
            {
                Id = "testClient2Id",
                UserId = user2.Id,
                FullName = user2.FullName
            };

            dbContext.Clients.Add(client2);

            dbContext.SaveChanges();
        }

        [Test]
        public void GetClientIdShouldReturnClientId()
        {
            var result = service.GetClientId("testUserId");
            Assert.That(result.GetType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetClientIdShouldReturnNullWhenUserNotExist()
        {
            var result = service.GetClientId("NotExistingUserId");
            Assert.IsTrue(result == null);
        }
    }
}
