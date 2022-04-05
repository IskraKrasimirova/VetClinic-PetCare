using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Doctors;

namespace VetClinic.Controllers
{
    public class DoctorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IDoctorService doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        public IActionResult All([FromQuery] AllDoctorsViewModel query)
        {
            var queryResult = doctorService.All(
                query.SearchTerm,
                query.Department,
                query.CurrentPage,
                AllDoctorsViewModel.DoctorsPerPage);

            var doctorsDepartments = doctorService.AllDepartments();

            query.TotalDoctors = queryResult.TotalDoctors;
            query.Doctors = queryResult.Doctors;
            query.Departments = doctorsDepartments;

            return View(query);
        }

        public IActionResult Available([FromQuery] AvailableDoctorsServiceModel query)
        {
            var queryResult = doctorService.ByDepartment(query);

            query.DepartmentId = queryResult.DepartmentId;
            query.Doctors = queryResult.Doctors;

            return View(query);
        }

        public IActionResult Details(string id, string information)
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
