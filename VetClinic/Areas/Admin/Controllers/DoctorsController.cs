using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Doctors;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Areas.Admin.Controllers
{
    public class DoctorsController : AdminController
    {
        private readonly IDoctorService doctorService;
        private readonly IDepartmentService departmentService;

        public DoctorsController(IDoctorService doctorService, IDepartmentService departmentService)
        {
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
        public IActionResult Add(DoctorFormModel doctorModel)
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

            this.TempData[GlobalMessageKey] = $"Successfully added doctor {doctorModel.FullName}.";

            return RedirectToAction("Details", new { Area = "", id = doctorId });
        }

        public IActionResult Edit(string id)
        {
            var doctor = doctorService.Details(id);

            return View(new DoctorFormModel
            {
                FullName = doctor.FullName,
                ProfileImage = doctor.ProfileImage,
                Description = doctor.Description,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                DepartmentId = doctor.DepartmentId,
                Departments = departmentService.GetAllDepartments()
            });
        }

        [HttpPost]
        public IActionResult Edit(string id, DoctorFormModel doctor)
        {
            if (!departmentService.DepartmentExists(doctor.DepartmentId))
            {
                this.ModelState.AddModelError(nameof(doctor.DepartmentId), "Department does not exist.");
            }

            if (!ModelState.IsValid)
            {
                doctor.Departments = this.departmentService.GetAllDepartments();

                return View(doctor);
            }

            var isEdited = doctorService.Edit(
                id,
                doctor.FullName,
                doctor.ProfileImage,
                doctor.Description,
                doctor.Email,
                doctor.PhoneNumber,
                doctor.DepartmentId);

            if (!isEdited)
            {
                return BadRequest();
            }

            this.TempData[GlobalMessageKey] = $"Successfully edited doctor {doctor.FullName}.";

            return RedirectToAction("Details", "Doctors", new { Area = "", id });
        }

        public IActionResult Delete(string id)
        {
            var doctor = this.doctorService.Details(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        [HttpPost]
        public IActionResult DeleteDoctor(string id)
        {
            var isDeleted = doctorService.Delete(id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            this.TempData[GlobalMessageKey] = "Successfully deleted a doctor.";

            return RedirectToAction("All", "Doctors", new { Area = "" });
        }
    }
}
