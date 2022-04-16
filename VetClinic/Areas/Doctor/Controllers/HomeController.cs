using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Extensions;

namespace VetClinic.Areas.Doctor.Controllers
{
    public class HomeController : DoctorsController
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            var userId = this.User.GetId();

            this.ViewBag.DoctorFullname = this.homeService.GetDoctorFullName(userId);

            return View();
        }
    }
}
