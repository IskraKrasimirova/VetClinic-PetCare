using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Doctors;
using VetClinic.Data.Models;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Areas.Admin.Controllers
{
    public class DoctorsController : AdminController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IDoctorService doctorService;
        private readonly IDepartmentService departmentService;

        public DoctorsController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IDoctorService doctorService, IDepartmentService departmentService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.doctorService = doctorService;
            this.departmentService = departmentService;
        }

        public IActionResult Add()
        {
            return View(new DoctorFormModel
            {
                Departments = this.departmentService.GetAllDepartments()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(DoctorFormModel doctorModel)
        {
            if (!departmentService.DepartmentExists(doctorModel.DepartmentId))
            {
                this.ModelState.AddModelError(nameof(doctorModel.DepartmentId), "Department does not exist.");
            }

            if (this.doctorService.DoctorExists(doctorModel.FullName, doctorModel.PhoneNumber))
            {
                this.ModelState.AddModelError(nameof(doctorModel.FullName), "The doctor already exists.");
            }

            if (!ModelState.IsValid)
            {
                doctorModel.Departments = this.departmentService.GetAllDepartments();

                return View(doctorModel);
            }

            var userId = this.doctorService.Register(doctorModel);

            var doctorId = this.doctorService.Create(
                doctorModel.FullName,
                doctorModel.ProfileImage,
                doctorModel.PhoneNumber,
                doctorModel.Email,
                doctorModel.Description,
                doctorModel.DepartmentId,
                userId);

            return RedirectToAction("All", "Doctors", new { Area = "" });
            //return RedirectToAction("Details", new { Area = "", id = doctorId });
        }
    }
}
