﻿using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Departments;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Areas.Admin.Controllers
{
    public class DepartmentsController : AdminController
    {
        private readonly IDepartmentService departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(DepartmentFormModel department)
        {
            if (this.departmentService.DepartmentExists(department.Name))
            {
                this.ModelState.AddModelError(nameof(department.Name), "The department already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var departmentId= this.departmentService.Create(
                department.Name,
                department.Image,
                department.Description);

            this.TempData[GlobalMessageKey] = $"Successfully added the department {department.Name}.";

            return RedirectToAction("Details", "Departments", new { Area = "", id = departmentId });
        }

        public IActionResult Edit(int id)
        {
            var department = departmentService.Details(id);

            return View(new DepartmentFormModel
            {
                Name = department.Name,
                Image = department.Image,
                Description = department.Description,
            });
        }

        [HttpPost]
        public IActionResult Edit(int id, DepartmentFormModel department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var isEdited = departmentService.Edit(
                id,
                department.Name,
                department.Image,
                department.Description);

            if (!isEdited)
            {
                return BadRequest();
            }

            this.TempData[GlobalMessageKey] = $"Successfully edited the department {department.Name}.";

            return RedirectToAction("Details", "Departments", new { Area = "", id  });
        }

        public IActionResult Delete(int id)
        {
            var department = this.departmentService.Details(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        public IActionResult DeleteDepartment(int id)
        {
            var isDeleted = departmentService.Delete(id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            this.TempData[GlobalMessageKey] = "Successfully deleted a department.";

            return RedirectToAction("All", "Departments", new { Area = "" });
        }
    }
}
