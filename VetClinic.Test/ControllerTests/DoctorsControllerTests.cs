using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using NUnit.Framework;
using VetClinic.Controllers;
using VetClinic.Core.Models.Doctors;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using DoctorService = VetClinic.Core.Services.DoctorService;

namespace VetClinic.Test.ControllerTests
{
    public class DoctorsControllerTests
    {
        private VetClinicDbContext dbContext;
        private ServiceService serviceService;
        private DepartmentService departmentService;
        private DoctorService service;
        private DoctorsController controller;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            serviceService = new ServiceService(dbContext);
            service = new DoctorService(UserManagerMock.Instance, dbContext);
            departmentService = new DepartmentService(dbContext, service, serviceService);
            controller = new DoctorsController(service, departmentService);
        }

        [Test]
        public void AllShouldReturnViewWithModel()
        {
            var expectedModel = new AllDoctorsViewModel
            {
                CurrentPage = 1,
                Departments = new List<string> { "TestDepartment1", "TestDepartment2" },
                Department = "TestDepartment1",
                TotalDoctors = 1,
                Doctors = new List<DoctorServiceModel>
                {
                    new DoctorServiceModel
                    {
                        Id = "TestDoctorId",
                        FullName = "TestDoctor FullName",
                        Department = "TestDepartment1"
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
                .BeOfType<AllDoctorsViewModel>();
        }

        [Test]
        public void AvailableShouldReturnViewWithModel()
        {
            var expectedModel = new AvailableDoctorsServiceModel
            {
                DepartmentId = 1,
                Doctors = new List<DoctorServiceModel>
                {
                    new DoctorServiceModel
                    {
                        Id = "TestDoctorId",
                        FullName = "TestDoctor FullName",
                        DepartmentId = 1,
                        Department = "TestDepartment1"
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
                .BeOfType<AvailableDoctorsServiceModel>();
        }

        [Test]
        public void DetailsReturnViewWithModelWhenDataIsValid()
        {
            var doctor = new Doctor
            {
                Id = "TestDoctorId",
                FullName = "TestDoctor FullName",
                DepartmentId = 1,
                Department = new Department
                {
                    Id = 1,
                    Name = "TestDepartment",
                    Image = "TestDepartmentImg.png",
                },
                UserId = "TestUserId",
                Description = "some description",
                Email = "testDoctor@petcare.com",
                PhoneNumber = "0888555666",
                ProfileImage = "testProfileImg.png"
            };
            dbContext.Doctors.Add(doctor);
            dbContext.SaveChanges();
            var result = controller.Details(doctor.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<DoctorDetailsServiceModel>();
        }

        [Test]
        public void DetailsReturnNotFoundWhenDoctorNotExist()
        {
            var result = controller.Details("TestDoctorId");
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
    }
}
