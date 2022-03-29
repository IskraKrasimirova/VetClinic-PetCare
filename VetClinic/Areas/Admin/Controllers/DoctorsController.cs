using Microsoft.AspNetCore.Mvc;

namespace VetClinic.Areas.Admin.Controllers
{
    public class DoctorsController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
