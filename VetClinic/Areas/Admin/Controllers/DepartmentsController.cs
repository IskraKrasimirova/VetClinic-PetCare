using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Departments;

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

            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Details", new {id = departmentId});
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

            return RedirectToAction("All", "Departments");
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = departmentService.Delete(id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Departments");
        }
    }
}
