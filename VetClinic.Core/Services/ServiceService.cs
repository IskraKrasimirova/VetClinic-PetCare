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

        public AllServicesViewModel All(string departmentName, string searchTerm)
        {
            var servicesQuery = this.data.Services.AsQueryable();

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                servicesQuery = servicesQuery.Where(s => s.Department.Name == departmentName);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                servicesQuery = servicesQuery.Where(s =>
                s.Department.Name.ToLower().Contains(searchTerm.ToLower()) ||
                s.Name.ToLower().Contains(searchTerm.ToLower()) ||
                s.Description.ToLower().Contains(searchTerm.ToLower()));
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

        public AvailableServicesViewModel ByDepartment(AvailableServicesViewModel query)
        {
            var servicesQuery = this.data.Services.AsQueryable();

            var availableServices = GetAvailableServices(query, servicesQuery);

            return availableServices;
        }

        public bool ServiceExists(string name)
        {
            return this.data.Services
                .Any(s => s.Name == name);
        }

        public int Create(
            string name,
            string description,
            decimal price,
            int departmentId)
        {
            var department = this.data.Departments.Find(departmentId);

            var service = new Service
            {
                Name = name,
                Description = description,
                Price = price,
                DepartmentId = departmentId
            };

            data.Services.Add(service);
            department.Services.Add(service);

            this.data.SaveChanges();

            return service.Id;
        }

        private IEnumerable<ServiceViewModel> GetServices(IQueryable<Service> serviceQuery)
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

        private AvailableServicesViewModel GetAvailableServices(AvailableServicesViewModel query, IQueryable<Service> servicesQuery)
        {
            return new AvailableServicesViewModel
            {
                DepartmentId = query.DepartmentId,
                Services = GetServices(servicesQuery
                .Where(d => d.DepartmentId == query.DepartmentId))
            };
        }
    }
}
