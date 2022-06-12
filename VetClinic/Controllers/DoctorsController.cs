using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Doctors;

namespace VetClinic.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService doctorService;
        private readonly IDepartmentService departmentService;

        public DoctorsController(IDoctorService doctorService, IDepartmentService departmentService)
        {
            this.doctorService = doctorService;
            this.departmentService = departmentService;
        }

        public IActionResult All([FromQuery] AllDoctorsViewModel query)
        {
            var queryResult = doctorService.All(
                query.Department,
                query.SearchTerm,
                query.CurrentPage,
                AllDoctorsViewModel.DoctorsPerPage);

            var doctorsDepartments = departmentService.AllDepartments();

            query.Departments = doctorsDepartments;
            query.TotalDoctors = queryResult.TotalDoctors;
            query.Doctors = queryResult.Doctors;
            query.SearchTerm = queryResult.SearchTerm;

            return View(query);
        }

        public IActionResult Available([FromQuery] AvailableDoctorsServiceModel query)
        {
            var queryResult = doctorService.ByDepartment(query);

            query.DepartmentId = queryResult.DepartmentId;
            query.Doctors = queryResult.Doctors;

            return View(query);
        }

        public IActionResult Details(string id)
        {
            var doctor = this.doctorService.Details(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
    }
}
