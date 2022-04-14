﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Appointments;
using VetClinic.Core.Models.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using static VetClinic.Common.GlobalConstants.FormattingConstants;
using static VetClinic.Common.DefaultHourSchedule;
using Microsoft.EntityFrameworkCore;

namespace VetClinic.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly VetClinicDbContext data;

        public AppointmentService(VetClinicDbContext data)
        {
            this.data = data;
        }
        public BookAppointmentServiceModel GetDoctorSchedule(string doctorId)
        {
            if (HourScheduleAsString == null)
            {
                SeedHourScheduleAsString();
            }

            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
            {
                return null;
            }

            var departmentId = doctor.DepartmentId;

            var department = this.data.Departments
                .FirstOrDefault(d => d.Id == departmentId);

            var services = this.data.Services
                .Where(s => s.DepartmentId == departmentId)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    DepartmentId = departmentId,
                    Department = department.Name
                })
                .ToList();

            //var doctorServices = doctor.DoctorServices;

            return new BookAppointmentServiceModel
            {
                DoctorId = doctorId,
                DoctorFullName = doctor.FullName,
                Services = services
            };
        }

        public DateTime TryToParseDate(string dateAsString, string hourAsString)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("bg-BG");

            if (!DateTime.TryParseExact(hourAsString, HourFormat, cultureInfo, DateTimeStyles.None, out DateTime parsedTime))
            {
                return DateTime.MinValue;
            }

            if (!DateTime.TryParseExact(dateAsString, NormalDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parseDate))
            {
                return DateTime.MinValue;
            }

            return parseDate.AddHours(parsedTime.Hour);
        }

        public string CheckDoctorAvailableHours(DateTime appointmentDateTime, string hourAsString, string doctorId)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("bg-BG");
            if (DateTime.TryParseExact(hourAsString, HourFormat, cultureInfo, DateTimeStyles.None, out _))
            {
                var doctorsQuery = this.data.Doctors
                    .Include(d => d.Appointments)
                    .AsQueryable();

                var isUnavailable = doctorsQuery
                   .Any(d => d.Id == doctorId && d.Appointments.Any(a => a.Date == appointmentDateTime));

                if (isUnavailable)
                {
                    return GetAvailableHours(appointmentDateTime, hourAsString, doctorId, doctorsQuery);
                }
            }

            return null;
        }

        public void AddNewAppointment
            (string clientId,
             string doctorId,
             int serviceId,
             string petId,
             DateTime appointmentDateTime,
             string hourAsString
             )
        {
            var client = this.data.Clients
                .FirstOrDefault(c => c.Id == clientId);

            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.Id == doctorId);

            var service = this.data.Services
                .FirstOrDefault(s => s.Id == serviceId);

            var pet = this.data.Pets
                .FirstOrDefault(p => p.Id == petId);

            var appointment = new Appointment()
            {
                Date = appointmentDateTime,
                Hour = hourAsString,
                ClientId = client.Id,
                Client = client,
                DoctorId = doctor.Id,
                Doctor = doctor,
                ServiceId = service.Id,
                Service = service,
                PetId = pet.Id,
                Pet = pet,
            };

            this.data.Appointments.Add(appointment);

            this.data.SaveChanges();
        }

        public IEnumerable<ServiceViewModel> AllServices(string doctorId)
        {
            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
            {
                return null;
            }

            var departmentId = doctor.DepartmentId;

            var department = this.data.Departments
                .FirstOrDefault(d => d.Id == departmentId);

            var services = this.data.Services
                .Where(s => s.DepartmentId == departmentId)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    DepartmentId = departmentId,
                    Department = department.Name
                })
                .ToList();

            return services;
        }

        public IEnumerable<UpcomingAppointmentServiceModel> GetUpcomingAppointments(string clientId)
        {
            if (!this.data.Clients.Any(c => c.Id == clientId))
            {
                return null;
            }

            var upcomingAppointments = this.data.Appointments
                .Where(a => a.ClientId == clientId && a.Date > DateTime.UtcNow)
                .Select(a => new UpcomingAppointmentServiceModel
                {
                    Id = a.Id,
                    Date = a.Date,
                    Hour = a.Hour,
                    PetName = a.Pet.Name,
                    ServiceName = a.Service.Name,
                    DoctorFullName = a.Doctor.FullName,
                    DoctorPhoneNumber = a.Doctor.PhoneNumber,
                })
                .OrderBy(a => a.Date)
                .ToList();

            return upcomingAppointments;
        }

        public IEnumerable<PastAppointmentServiceModel> GetPastAppointments(string clientId)
        {
            if (!this.data.Clients.Any(c => c.Id == clientId))
            {
                return null;
            }

            var pastAppointments = this.data.Appointments
                .Where(a => a.ClientId == clientId && a.Date < DateTime.UtcNow)
                .Select(a => new PastAppointmentServiceModel
                {
                    Id = a.Id,
                    Date = a.Date,
                    Hour = a.Hour,
                    PetName = a.Pet.Name,
                    ServiceName = a.Service.Name,
                    DoctorFullName = a.Doctor.FullName,
                    DoctorPhoneNumber= a.Doctor.PhoneNumber,
                    ClientId = clientId,
                    PetId = a.Pet.Id,
                    DoctorId = a.Doctor.Id,
                    ServiceId = a.Service.Id,
                })
                .OrderByDescending(a => a.Date)
                .ToList();

            return pastAppointments;
        }

        private static string GetAvailableHours(
             DateTime appointmentDateTime,
             string appointmentHour,
             string doctorId,
            IQueryable<Doctor> doctorsQuery)
        {
            var bookedHours = doctorsQuery
                .FirstOrDefault(d => d.Id == doctorId)
                ?.Appointments
                .Where(a => a.Date.Day == appointmentDateTime.Day &&
                             a.Date.Month == appointmentDateTime.Month &&
                             a.Date.Year == appointmentDateTime.Year &&
                             a.Hour == appointmentHour)
                .Select(a => a.Hour)
                .ToList();

            if (HourScheduleAsString == null)
            {
                SeedHourScheduleAsString();
            }

            var defaultHourSchedule = new List<string>(HourScheduleAsString);

            foreach (var booked in bookedHours)
            {
                if (defaultHourSchedule.Contains(booked))
                {
                  defaultHourSchedule.Remove(booked);
                }
            }

            var availabeHoursMessage = string.Empty;

            if (defaultHourSchedule == null)
            {
                availabeHoursMessage = "There are no available hours for that day.";
            }
            else
            {
                var availableHours = string.Join(" ", defaultHourSchedule);
                availabeHoursMessage = $"The available hours for the date {appointmentDateTime.ToString(NormalDateFormat)} are: {availableHours}";
            }

            return availabeHoursMessage;
        }
    }
}