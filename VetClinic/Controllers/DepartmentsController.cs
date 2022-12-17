using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Departments;

namespace VetClinic.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IMemoryCache memoryCache;

        public DepartmentsController(IDepartmentService departmentService, IMemoryCache memoryCache)
        {
            this.departmentService = departmentService;
            this.memoryCache = memoryCache;
        }

        public IActionResult All()
        {
            var allDepartments = this.memoryCache.Get<List<DepartmentListingViewModel>>("AllDepartmentsCacheKey");

            if (allDepartments == null)
            {
                allDepartments = this.departmentService.GetAllDepartments().ToList();
                this.memoryCache.Set("AllDepartmentsCacheKey", allDepartments, TimeSpan.FromMinutes(5));
            }

            return View(allDepartments);
        }

        //public IActionResult All()
        //{
        //    var allDepartments = this.departmentService.GetAllDepartments();

        //    if (allDepartments == null)
        //    {
        //        this.ModelState.AddModelError(String.Empty, "No departments are found.");
        //    }

        //    return View(allDepartments);
        //}

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
