using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Doctors;
using VetClinic.Data;
using VetClinic.Data.Models;

namespace VetClinic.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly VetClinicDbContext data;
        private readonly IDepartmentService departmentService;

        public DoctorService(VetClinicDbContext data, IDepartmentService departmentService)
        {
            this.data = data;
            this.departmentService = departmentService;
        }

        public AllDoctorsViewModel All(string departmentName, string searchTerm, int currentPage = 1,
            int doctorsPerPage = int.MaxValue)
        {
            var doctorsQuery = this.data.Doctors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                doctorsQuery = doctorsQuery.Where(d => d.Department.Name == departmentName);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                doctorsQuery = doctorsQuery.Where(d =>
                d.Department.Name.ToLower().Contains(searchTerm.ToLower()) ||
                d.FullName.ToLower().Contains(searchTerm.ToLower()) ||
                d.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalDoctors = doctorsQuery.Count();

            var doctors = GetDoctors(doctorsQuery
                .Skip((currentPage - 1) * doctorsPerPage)
                .Take(doctorsPerPage)
                .OrderBy(d => d.DepartmentId)
                .ThenBy(d => d.FullName));

            var doctorsDepartments = departmentService.AllDepartments();

            return new AllDoctorsViewModel
            {
                CurrentPage = currentPage,
                TotalDoctors = totalDoctors,
                Doctors = doctors,
                Departments = doctorsDepartments,
                Department = departmentName,
                SearchTerm = searchTerm
            };
        }

        public AvailableDoctorsServiceModel ByDepartment(AvailableDoctorsServiceModel query)
        {
            var doctorsQuery = this.data.Doctors.AsQueryable();

            var doctors = GetAvailableDoctors(query, doctorsQuery);

            return doctors;
        }
        
        public DoctorDetailsServiceModel Details(string id)
        {
            return this.data.Doctors
                .Where(d => d.Id == id)
                .Select(d => new DoctorDetailsServiceModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ProfileImage = d.ProfileImage,
                    Department = d.Department.Name,
                    Description = d.Description,
                    Email = d.Email,
                    PhoneNumber = d.PhoneNumber,
                    UserId = d.UserId
                })
                .FirstOrDefault();
        }

        private IEnumerable<DoctorServiceModel> GetDoctors(IQueryable<Doctor> doctorQuery)
        {
            return doctorQuery
                .Select(d => new DoctorServiceModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ProfileImage = d.ProfileImage,
                    Department = d.Department.Name,
                    DepartmentId = d.DepartmentId
                })
                .ToList();
        }

        private AvailableDoctorsServiceModel GetAvailableDoctors(AvailableDoctorsServiceModel query, IQueryable<Doctor> doctorsQuery)
        {
            return new AvailableDoctorsServiceModel
            {
                DepartmentId = query.DepartmentId,
                Doctors = GetDoctors(doctorsQuery
                .Where(d => d.DepartmentId == query.DepartmentId))
            };
        }
    }
}
