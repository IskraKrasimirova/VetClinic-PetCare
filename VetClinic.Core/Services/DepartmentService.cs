using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
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

        public int Create(string name, string description)
        {
            var department = new Department
            {
                Name = name,
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
    }
}
