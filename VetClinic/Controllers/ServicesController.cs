﻿using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Services;

namespace VetClinic.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService service;
        private readonly IDepartmentService departmentService;

        public ServicesController(IServiceService service, IDepartmentService departmentService)
        {
            this.service = service;
            this.departmentService = departmentService;
        }

        //public IActionResult All()
        //{
        //    var allServices = this.service.GetAllServices();

        //    if (!allServices.Any())
        //    {
        //        this.ModelState.AddModelError(String.Empty, "No services are found.");
        //    }
        //    return View(allServices);
        //}

        public IActionResult All([FromQuery] AllServicesViewModel query)
        {
            var queryResult = service.All(query.Department);

            var servicesDepartments = departmentService.AllDepartments();

            query.Departments = servicesDepartments;
            query.Services = queryResult.Services;

            return View(query);
        }
    }
}