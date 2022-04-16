using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Extensions;

namespace VetClinic.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            var userId = this.User.GetId();

            this.ViewBag.AdminFullname = this.homeService.GetAdminFullName(userId);

            return View();
        }
    }
}
