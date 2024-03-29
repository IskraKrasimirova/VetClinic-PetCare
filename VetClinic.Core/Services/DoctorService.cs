﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Doctors;
using VetClinic.Data;
using VetClinic.Data.Models;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly UserManager<User> userManager;
        private readonly VetClinicDbContext data;

        public DoctorService(UserManager<User> userManager, VetClinicDbContext data)
        {
            this.userManager = userManager;
            this.data = data;
        }
        
        public AllDoctorsViewModel All(string departmentName, string searchTerm, int currentPage = 1, int doctorsPerPage = int.MaxValue)
        {
            var doctorsQuery = this.data.Doctors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                doctorsQuery = doctorsQuery
                    .Where(d => d.Department.Name == departmentName);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                doctorsQuery = doctorsQuery.Where(d =>
                d.Department.Name.ToLower().Contains(searchTerm.Trim().ToLower()) ||
                d.FullName.ToLower().Contains(searchTerm.Trim().ToLower()) ||
                d.Description.ToLower().Contains(searchTerm.Trim().ToLower()));
            }

            doctorsQuery = doctorsQuery
                .OrderBy(d => d.DepartmentId)
                .ThenBy(d => d.FullName);

            var totalDoctors = doctorsQuery.Count();

            var doctors = GetDoctors(doctorsQuery
                .Skip((currentPage - 1) * doctorsPerPage)
                .Take(doctorsPerPage));

            var doctorsDepartments = this.AllDepartments();

            return new AllDoctorsViewModel
            {
                CurrentPage = currentPage,
                TotalDoctors = totalDoctors,
                Doctors = doctors,
                Departments = doctorsDepartments,
                Department = departmentName,
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

        public bool DoctorExists(string fullName, string phoneNumber)
        {
            return this.data.Doctors
                .Any(d => d.FullName == fullName && d.PhoneNumber == phoneNumber);
        }

        public bool Edit(
                string id,
                string fullName,
                string profileImage,
                string description,
                string email,
                string phoneNumber,
                int departmentId)
        {
            var doctor = this.data.Doctors.Find(id);

            if (doctor == null)
            {
                return false;
            }

            doctor.FullName = fullName;
            doctor.ProfileImage = profileImage;
            doctor.Description = description;
            doctor.Email = email;
            doctor.PhoneNumber = phoneNumber;
            doctor.DepartmentId = departmentId;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var doctor = this.data.Doctors
                .Include(d => d.Appointments)
                .Include(d => d.Prescriptions)
                .FirstOrDefault(d => d.Id == id);

            if (doctor == null)
            {
                return false;
            }

            var prescriptions = doctor.Prescriptions.ToArray();
            this.data.Prescriptions.RemoveRange(prescriptions);
            this.data.SaveChanges();

            var appointments = doctor.Appointments.ToArray();
            this.data.Appointments.RemoveRange(appointments);
            this.data.SaveChanges();

            var pets = this.data.PetDoctors
                .Where(d => d.DoctorId == id)
                .ToArray();
            this.data.PetDoctors.RemoveRange(pets);
            this.data.SaveChanges();

            var services = this.data.DoctorServices
                .Where(d => d.DoctorId == id)
                .ToArray();
            this.data.DoctorServices.RemoveRange(services);
            this.data.SaveChanges();

            var user = this.data.Users
                .FirstOrDefault(u => u.Id == doctor.UserId);

            Task
                .Run(async () =>
                {
                    await this.userManager.RemoveFromRoleAsync(user, DoctorRoleName);
                })
                .GetAwaiter()
                .GetResult();

            this.data.Doctors.Remove(doctor);
            this.data.SaveChanges();

            this.data.Users.Remove(user);
            this.data.SaveChanges();

            var department = this.data.Departments
                .FirstOrDefault(d => d.Id == doctor.DepartmentId);

            department.Doctors.Remove(doctor);
            this.data.SaveChanges();

            return true;
        }


        public string Register(DoctorFormModel doctorModel)
        {
            var user = new User()
            {
                Email = doctorModel.Email,
                UserName = doctorModel.Email,
                PhoneNumber = doctorModel.PhoneNumber,
                FullName = doctorModel.FullName,
            };

            Task
                .Run(async () =>
                {
                    await userManager.CreateAsync(user, DefaultPassword);

                    await userManager.AddToRoleAsync(user, DoctorRoleName);
                })
                .GetAwaiter()
                .GetResult();

            return user.Id;
        }

        public string Create(
                string fullName,
               string profileImage,
               string phoneNumber,
               string email,
               string description,
               int departmentId,
               string userId)
        {
            var doctor = new Doctor
            {
                FullName = fullName,
                ProfileImage = profileImage,
                PhoneNumber = phoneNumber,
                Email = email,
                Description = description,
                DepartmentId = departmentId,
                UserId = userId
            };

            this.data.Doctors.Add(doctor);

            this.data.SaveChanges();

            return doctor.Id;
        }

        public DoctorServiceModel GetDoctor(string userId)
        {
            return this.data.Doctors
                .Where(d => d.UserId == userId)
                .Select(d => new DoctorServiceModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    DepartmentId = d.DepartmentId,
                    Department = d.Department.Name
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

        private IEnumerable<string> AllDepartments()
        {
            return this.data.Departments
                .Select(d => d.Name)
                .Distinct()
                .ToList();
        }
    }
}
