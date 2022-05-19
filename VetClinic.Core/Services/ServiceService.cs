using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Services;
using VetClinic.Data;
using VetClinic.Data.Models;

namespace VetClinic.Core.Services
{
    public class ServiceService : IServiceService
    {
        private readonly VetClinicDbContext data;

        public ServiceService(VetClinicDbContext data)
        {
            this.data = data;
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

            var servicesDepartments = this.AllDepartments();

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

        public ServiceViewModel Details(int id)
        {
            return this.data.Services
                .Where(s => s.Id == id)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    DepartmentId = s.DepartmentId,
                    Department = s.Department.Name
                })
                .FirstOrDefault();
        }

        public bool Edit(int id, string name, string description, decimal price, int departmentId)
        {
            var service = this.data.Services.Find(id);

            if (service == null)
            {
                return false;
            }

            service.Name = name;
            service.Description = description;
            service.Price = price;
            service.DepartmentId = departmentId;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var service = this.data.Services
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if (service == null)
            {
                return false;
            }

            var appointments = this.data.Appointments
                .Where(a => a.ServiceId == id)
                .ToArray();

            this.data.Appointments.RemoveRange(appointments);
            this.data.SaveChanges();

            var doctorServices = this.data.DoctorServices
                .Where(s => s.ServiceId == id)
                .ToArray();

            this.data.DoctorServices.RemoveRange(doctorServices);
            this.data.SaveChanges();

            var petServices = this.data.PetServices
                .Where(s => s.ServiceId == id)
                .ToArray();

            this.data.PetServices.RemoveRange(petServices);
            this.data.SaveChanges();

            this.data.Services.Remove(service);
            this.data.SaveChanges();

            var department = this.data.Departments
                .FirstOrDefault(d => d.Id == service.DepartmentId);

            department.Services.Remove(service);
            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<ServiceViewModel> AllServices()
        {
            return this.data.Services
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    DepartmentId = s.DepartmentId,
                    Department = s.Department.Name,
                })
                .ToList();
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

        private IEnumerable<string> AllDepartments()
        {
            return this.data.Departments
                .Select(d => d.Name)
                .Distinct()
                .ToList();
        }
    }
}
