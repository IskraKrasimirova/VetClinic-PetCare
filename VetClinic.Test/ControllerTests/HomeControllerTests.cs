using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Controllers;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Test.ControllerTests
{
    public class HomeControllerTests
    {
        private VetClinicDbContext dbContext;
        private HomeService service;
        private HomeController controller;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new HomeService(dbContext);
            controller = new HomeController(service);
        }

        [Test]
        public void IndexShouldReturnView()
        {
            var user = new User()
            {
                FullName = "TestUserFullName"
            };
            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = controller.Index();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .ToString()
                .Equals(result.GetType().Name);
        }

        [Test]
        public void IndexShouldReturnViewWhenUserIsClient()
        {
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
                FullName = user.FullName,
            };

            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(ClientRoleName)
            };
            var result = controller.Index();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>();
        }

        [Test]
        public void IndexShouldReturnViewInDoctorAreaWhenUserIsDoctor()
        {
            var user = new User
            {
                Id = "TestUserId",
                Email = "testDoctor@vetclinic.com",
                UserName = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName"
            };

            dbContext.Users.Add(user);

            var doctor = new Doctor
            {
                Id = "testDoctorId",
                UserId = user.Id,
                Email = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName",
                PhoneNumber = "0111222333",
                ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/testDoctor.png",
                DepartmentId = 1,
                Description = "TestDescription"
            };

            dbContext.Doctors.Add(doctor);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(DoctorRoleName),
            };
            var result = controller.Index();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>()
                .Which
                .ToString()
                .Equals(result.GetType().Name);
        }

        [Test]
        public void IndexShouldReturnViewInAdminAreaWhenUserIsAdmin()
        {
            var user = new User
            {
                Id = "TestUserId",
                Email = "testAdmin@vetclinic.com",
                UserName = "testAdmin@vetclinic.com",
                FullName = "TestAdminFullName"
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(AdminRoleName)
            };

            var result = controller.Index();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>()
                .Which
                .ToString()
                .Equals(result.GetType().Name);
        }

        [Test]
        public void ErrorShouldReturnView()
        {
            var result = controller.Error();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>();
        }

        [Test]
        public void PrivacyShouldReturnView()
        {
            var result = controller.Privacy();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>();
        }
    }
}
