using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Services;
using VetClinic.Data;
using VetClinic.Data.Models;

namespace VetClinic.Core.Services
{
    public class ServiceService : IServiceService
    {
        private readonly VetClinicDbContext data;
        private readonly IDepartmentService departmentService;

        public ServiceService(VetClinicDbContext data, IDepartmentService departmentService)
        {
            this.data = data;
            this.departmentService = departmentService;
        }

        //public IEnumerable<ServiceViewModel> GetAllServices()
        //{
        //    if (!this.data.Services.Any())
        //    {
        //        return null;
        //    }

        //    var allServices = this.data.Services
        //        .Select(s => new ServiceViewModel
        //        {
        //            Id = s.Id,
        //            Name = s.Name,
        //            Description = s.Description,
        //            Price = s.Price,
        //            Department = s.Department.Name
        //        })
        //        .ToList();

        //    return allServices;
        //}

        public AllServicesViewModel All(string departmentName)
        {
            var servicesQuery = this.data.Services.AsQueryable();

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                servicesQuery = servicesQuery.Where(s => s.Department.Name == departmentName);
            }

            var services = GetServices(servicesQuery
                .OrderBy(s => s.DepartmentId)
                .ThenBy(s => s.Id));

            var servicesDepartments = departmentService.AllDepartments();

            return new AllServicesViewModel
            {
                Services = services,
                Departments = servicesDepartments,
                Department = departmentName,
            };
        }

        private IEnumerable<ServiceViewModel> GetServices(IOrderedQueryable<Service> serviceQuery)
        {
            return serviceQuery
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    Department = s.Department.Name,
                    DepartmentId = s.DepartmentId
                })
                .ToList();
        }
    }
}
