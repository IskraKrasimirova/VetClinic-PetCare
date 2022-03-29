using Microsoft.AspNetCore.Mvc;

namespace VetClinic.Areas.Admin.Controllers
{
    public class ServicesController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
