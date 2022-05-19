using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;

namespace VetClinic.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public IActionResult All()
        {
            var allDepartments = this.departmentService.GetAllDepartments();

            if (!allDepartments.Any())
            {
                this.ModelState.AddModelError(String.Empty, "No departments are found.");
            }

            return View(allDepartments);
        }

        public IActionResult Details(int id)
        {
            var department = this.departmentService.Details(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }
    }
}
