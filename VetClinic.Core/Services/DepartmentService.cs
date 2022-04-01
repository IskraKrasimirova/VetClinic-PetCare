using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Departments;
using VetClinic.Core.Models.Doctors;
using VetClinic.Data;
using VetClinic.Data.Models;

namespace VetClinic.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly VetClinicDbContext data;

        public DepartmentService(VetClinicDbContext data)
        {
            this.data = data;
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
                .Where(d => d.Id == id)
                .FirstOrDefault();

            if (department == null)
            {
                return false;
            }

            this.data.Departments.Remove(department);
            this.data.SaveChanges();

            return true;
        }
    }
}
