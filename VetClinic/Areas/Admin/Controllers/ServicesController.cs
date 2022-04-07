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

        public IActionResult Edit(int id)
        {
            var serviceModel = service.Details(id);

            return View(new ServiceFormModel
            {
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                Price = serviceModel.Price,
                DepartmentId = serviceModel.DepartmentId,
                Departments = departmentService.GetAllDepartments()
            });
        }

        [HttpPost]
        public IActionResult Edit(int id, ServiceFormModel serviceModel)
        {
            if (!departmentService.DepartmentExists(serviceModel.DepartmentId))
            {
                this.ModelState.AddModelError(nameof(serviceModel.DepartmentId), "Department does not exist.");
            }

            if (!ModelState.IsValid)
            {
                serviceModel.Departments = this.departmentService.GetAllDepartments();

                return View(serviceModel);
            }

            var isEdited = service.Edit(
                id,
                serviceModel.Name,
                serviceModel.Description,
                serviceModel.Price,
                serviceModel.DepartmentId);

            if (!isEdited)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", "Services", new { id });
        }

        public IActionResult Details(int id)
        {
            var serviceModel = this.service.Details(id);

            if (serviceModel == null)
            {
                return NotFound();
            }

            return View(serviceModel);
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = service.Delete(id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Services", new { Area = "" });
        }
    }
}
