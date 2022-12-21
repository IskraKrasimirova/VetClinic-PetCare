using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using NUnit.Framework;
using VetClinic.Controllers;
using VetClinic.Core.Models.Services;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Test.Mocks;

namespace VetClinic.Test.ControllerTests
{
    public class ServicesControllerTests
    {
        private VetClinicDbContext dbContext;
        private ServiceService service;
        private DoctorService doctorService;
        private DepartmentService departmentService;
        private ServicesController controller;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            service = new ServiceService(dbContext);
            doctorService = new DoctorService(UserManagerMock.Instance, dbContext);
            departmentService = new DepartmentService(dbContext, doctorService, service);
            controller = new ServicesController(service, departmentService);
        }

        [Test]
        public void AllShouldReturnViewWithModel()
        {
            var expectedModel = new AllServicesViewModel
            {
                Departments = new List<string> { "TestDepartment1", "TestDepartment2" },
                Department = "TestDepartment1",
                DepartmentId = 1,
                Services = new List<ServiceViewModel>
                {
                    new ServiceViewModel
                    {
                        Id = 1,
                        Name = "TestService",
                        DepartmentId = 1,
                        Department = "TestDepartment1",
                        Description = "description",
                        Price = 10.0M
                    }
                }
            };
            var result = controller.All(expectedModel);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<AllServicesViewModel>();
        }

        [Test]
        public void AvailableShouldReturnViewWithModel()
        {
            var expectedModel = new AvailableServicesViewModel
            {
                DepartmentId = 1,
                Services = new List<ServiceViewModel>
                {
                    new ServiceViewModel
                    {
                        Id = 1,
                        Name = "TestService",
                        DepartmentId = 1,
                        Department = "TestDepartment1",
                        Description = "description",
                        Price = 10.0M
                    }
                }
            };
            var result = controller.Available(expectedModel);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<AvailableServicesViewModel>();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
