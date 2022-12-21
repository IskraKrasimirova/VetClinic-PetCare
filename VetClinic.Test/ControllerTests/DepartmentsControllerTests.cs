using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using FluentAssertions;
using NUnit.Framework;
using VetClinic.Controllers;
using VetClinic.Core.Models.Departments;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using DoctorService = VetClinic.Core.Services.DoctorService;

namespace VetClinic.Test.ControllerTests
{
    public class DepartmentsControllerTests
    {
        private VetClinicDbContext dbContext;
        private DepartmentService service;
        private DoctorService doctorService;
        private ServiceService serviceService;
        private DepartmentsController controller;
        private IMemoryCache memoryCache;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            doctorService = new DoctorService(UserManagerMock.Instance, dbContext);
            serviceService = new ServiceService(dbContext);
            service = new DepartmentService(dbContext, doctorService, serviceService);
            memoryCache = new MemoryCache(optionsAccessor: new MemoryCacheOptions());
            controller = new DepartmentsController(service, memoryCache);
        }

        [Test]
        public void AllShouldReturnViewWithModelWhenDataIsValid()
        {
            GetDbContextWithDepartments();
            var result = controller.All();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<List<DepartmentListingViewModel>>()
                .Which
                .Count
                .Should()
                .BeGreaterThanOrEqualTo(2);
        }

        //[Test]
        //public void AllShouldReturnViewWithErrorMessageWhenNoDepartments()
        //{
        //    var departments = new List<Department>();
        //    dbContext.Departments.AddRange(departments);
        //    dbContext.SaveChanges();
        //    var result = controller.All();
        //    result
        //        .Should()
        //        .NotBeNull()
        //        .And
        //        .BeOfType<ViewResult>()
        //        .Which
        //        .ViewData
        //        .ModelState
        //        .ErrorCount
        //        .Should()
        //        .BeGreaterThanOrEqualTo(1);
        //}

        [Test]
        public void DetailsShouldReturnViewWhenDepartmentExists()
        {
            GetDbContextWithDepartments();
            var result = controller.Details(1);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<DepartmentDetailsServiceModel>();
        }


        [Test]
        public void DetailsShouldReturnNotFoundWhenDepartmentNotExist()
        {
            GetDbContextWithDepartments();
            var result = controller.Details(10);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<NotFoundResult>();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private void GetDbContextWithDepartments()
        {
            var departments = new List<Department>()
            {
                new Department
                {
                    Id = 1,
                    Name = "TestDepartmentName",
                    Image = "TestDepartmentImg.png",
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
                },
                new Department
                {
                    Id = 2,
                    Name = "TestDepartmentName2",
                    Image = "TestDepartmentImg2.png",
                    Services = new List<Service>
                    {
                        new Service
                        {
                            Id = 3,
                            Name = "TestService3"
                        },
                        new Service
                        {
                            Id = 4,
                            Name = "TestService4"
                        }
                    }
                }
           };

            dbContext.Departments.AddRange(departments);

            dbContext.SaveChanges();
        }
    }
}
