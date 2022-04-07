using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Services;

namespace VetClinic.Areas.Admin.Controllers
{
    public class ServicesController : AdminController
    {
        private readonly IServiceService service;
        private readonly IDepartmentService departmentService;

        public ServicesController(IServiceService service, IDepartmentService departmentService)
        {
            this.service = service;  
            this.departmentService = departmentService;
        }

        public IActionResult Add()
        {
            return View(new ServiceFormModel
            {
                Departments = this.departmentService.GetAllDepartments()
            });
        }

        [HttpPost]
        public IActionResult Add(ServiceFormModel serviceModel)
        {
            if (!departmentService.DepartmentExists(serviceModel.DepartmentId))
            {
                this.ModelState.AddModelError(nameof(serviceModel.DepartmentId), "Department does not exist.");
            }

            if (this.service.ServiceExists(serviceModel.Name))
            {
                this.ModelState.AddModelError(nameof(serviceModel.Name), "The service already exists.");
            }

            if (!ModelState.IsValid)
            {
                serviceModel.Departments = this.departmentService.GetAllDepartments();

                return View(serviceModel);
            }

            var serviceId = this.service.Create(
                serviceModel.Name,
                serviceModel.Description,
                serviceModel.Price,
                serviceModel.DepartmentId);

            return RedirectToAction("All", "Services", new {Area = ""});
            //return RedirectToAction("Details", new { id = serviceId });
        }
    }
}
