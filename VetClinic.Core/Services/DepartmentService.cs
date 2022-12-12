using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Departments;
using VetClinic.Data;
using VetClinic.Data.Models;

namespace VetClinic.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly VetClinicDbContext data;
        private readonly IDoctorService doctorService;
        private readonly IServiceService serviceService;

        public DepartmentService(VetClinicDbContext data, IDoctorService doctorService, IServiceService serviceService)
        {
            this.data = data;
            this.doctorService = doctorService;
            this.serviceService = serviceService;
        }

        public int Create(string name, string image, string description)
        {
            var department = new Department
            {
                Name = name,
                Image = image,
                Description = description
            };

            this.data.Departments.Add(department);
            this.data.SaveChanges();

            return department.Id;
        }

        public bool DepartmentExists(string name)
        {
            return this.data.Departments
                .Any(d => d.Name == name);
        }

        public bool DepartmentExists(int id)
        {
            return this.data.Departments
                .Any(d => d.Id == id);
        }

        public IEnumerable<string> AllDepartments()
        {
            return this.data.Departments
                .Select(d => d.Name)
                .Distinct()
                .ToList();
        }

        public IEnumerable<DepartmentListingViewModel> GetAllDepartments()
        {
            if (!this.data.Departments.Any())
            {
                return null;
            }

            var allDepartments = this.data.Departments
                .Select(d => new DepartmentListingViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Image = d.Image,
                    Description = d.Description
                })
                .ToList();

            return allDepartments;
        }

        public DepartmentDetailsServiceModel Details(int id)
        {
            return this.data.Departments
                .Where(d => d.Id == id)
                .Select(d => new DepartmentDetailsServiceModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Image = d.Image,
                    Description = d.Description,
                })
                .FirstOrDefault();
        }

        public bool Edit(int id, string name, string image, string description)
        {
            var department = this.data.Departments.Find(id);

            if (department == null)
            {
                return false;
            }

            department.Name = name;
            department.Image = image;
            department.Description = description;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var department = this.data.Departments
                .Include(d => d.Doctors)
                .Include(d => d.Services)
                .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return false;
            }

            var doctorIds = this.data.Doctors
                .Where(d => d.DepartmentId == id)
                .Select(d => new {d.Id})
                .ToArray();

            var serviceIds = this.data.Services
                .Where(s => s.DepartmentId == id)
                .Select(s => new { s.Id })
                .ToArray();

            foreach (var doctor in doctorIds)
            {
                this.doctorService.Delete(doctor.Id);
            }

            foreach (var service in serviceIds)
            {
                this.serviceService.Delete(service.Id);
            }

            this.data.Departments.Remove(department);
            this.data.SaveChanges();

            return true;
        }
    }
}
