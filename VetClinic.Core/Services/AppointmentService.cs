using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Appointments;
using VetClinic.Core.Models.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using static VetClinic.Common.DefaultHourSchedule;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

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

        public void AddNewAppointment(
            string clientId,
             string doctorId,
             int serviceId,
             string petId,
             DateTime appointmentDateTime,
             string hourAsString)
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
                .Where(a => a.ClientId == clientId && a.Date >= DateTime.UtcNow)
                .Select(a => new UpcomingAppointmentServiceModel
                {
                    Id = a.Id,
                    Date = a.Date,
                    Hour = a.Hour,
                    PetName = a.Pet.Name,
                    DepartmentName = this.data.Services
                                        .Where(s => s.Id == a.ServiceId)
                                        .Select(s => s.Department.Name)
                                        .FirstOrDefault(),
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
                .OrderByDescending(a => a.Date)
                .Select(a => new PastAppointmentServiceModel
                {
                    Id = a.Id,
                    Date = a.Date,
                    Hour = a.Hour,
                    PetName = a.Pet.Name,
                    DepartmentName = this.data.Services
                                        .Where(s => s.Id == a.ServiceId)
                                        .Select(s => s.Department.Name)
                                        .FirstOrDefault(),
                    ServiceName = a.Service.Name,
                    DoctorFullName = a.Doctor.FullName,
                    DoctorPhoneNumber = a.Doctor.PhoneNumber,
                    ClientId = clientId,
                    PetId = a.Pet.Id,
                    DoctorId = a.Doctor.Id,
                    ServiceId = a.Service.Id,
                    PrescriptionId = a.PrescriptionId
                })
                .ToList();

            return pastAppointments;
        }

        public CancelAppointmentServiceModel GetAppointmentForCancel(string appointmentId)
        {
            var appointment = this.data.Appointments
                .FirstOrDefault(a => a.Id == appointmentId);

            if (appointment == null || appointment.Date < DateTime.UtcNow.Date)
            {
                return null;
            }

            var petId = appointment.PetId;
            var doctorId = appointment.DoctorId;
            var serviceId = appointment.ServiceId;
            var clientId = appointment.ClientId;

            if (clientId == null || petId == null || doctorId == null || serviceId == 0)
            {
                return null;
            }

            var pet = this.data.Pets.Find(petId);
            var doctor = this.data.Doctors.Find(doctorId);
            var service = this.data.Services.Find(serviceId);
            var client = this.data.Clients.Find(clientId);

            var canceledAppointment = new CancelAppointmentServiceModel
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Hour = appointment.Hour,
                PetName = pet.Name,
                ClientFullName = client.FullName,
                DoctorFullName = doctor.FullName,
                ServiceName = service.Name,
            };

            return canceledAppointment;
        }

        public bool Delete(string appointmentId)
        {
            var appointment = this.data.Appointments
                .Where(a => a.Id == appointmentId)
                .FirstOrDefault();

            if (appointment == null)
            {
                return false;
            }

            this.data.Appointments.Remove(appointment);
            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<UpcomingAppointmentServiceModel> GetDoctorUpcomingAppointments(string userId)
        {
            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.UserId == userId);

            if (doctor == null)
            {
                return null;
            }

            var upcomingAppointments = this.data.Appointments
                .Where(a => a.DoctorId == doctor.Id && a.Date >= DateTime.UtcNow)
                .Select(a => new UpcomingAppointmentServiceModel
                {
                    Id = a.Id,
                    Date = a.Date,
                    Hour = a.Hour,
                    ServiceName = a.Service.Name,
                    PetName = a.Pet.Name,
                    PetType = a.Pet.PetType.Name,
                    ClientFullName = a.Client.FullName,
                    ClientPhoneNumber = this.data.Users
                                        .FirstOrDefault(u => u.FullName == a.Client.FullName)
                                        .PhoneNumber
                })
                .OrderBy(a => a.Date)
                .ToList();

            return upcomingAppointments;
        }

        public IEnumerable<PastAppointmentServiceModel> GetDoctorPastAppointments(string userId)
        {
            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.UserId == userId);

            if (doctor == null)
            {
                return null;
            }

            var pastAppointments = this.data.Appointments
                .Where(a => a.DoctorId == doctor.Id && a.Date < DateTime.UtcNow)
                .Select(a => new PastAppointmentServiceModel
                {
                    Id = a.Id,
                    Date = a.Date,
                    Hour = a.Hour,
                    PetName = a.Pet.Name,
                    PetType = a.Pet.PetType.Name,
                    ServiceName = a.Service.Name,
                    ClientFullName = a.Client.FullName,
                    ClientPhoneNumber = this.data.Users
                                .FirstOrDefault(u => u.FullName == a.Client.FullName)
                                .PhoneNumber,
                    ClientId = this.data.Pets
                                .Where(p => p.Id == a.PetId)
                                .Select(p => p.ClientId)
                                .ToString(),
                    PetId = a.Pet.Id,
                    DoctorId = a.Doctor.Id,
                    ServiceId = a.Service.Id,
                    PrescriptionId = a.PrescriptionId
                })
                .OrderByDescending(a => a.Date)
                .ToList();

            return pastAppointments;
        }

        public PastAppointmentServiceModel GetPastAppointment(string appointmentId)
        {
            var appointment = this.data.Appointments
                .Where(a => a.Id == appointmentId && a.Date < DateTime.UtcNow)
                .Select(a => new PastAppointmentServiceModel
                {
                    Date = a.Date,
                    DoctorId = a.DoctorId,
                    PetId = a.PetId,
                    PetName = a.Pet.Name,
                    ClientId = a.ClientId,
                    ServiceId = a.ServiceId,
                    ServiceName = a.Service.Name,
                    DoctorFullName = a.Doctor.FullName,
                    PrescriptionId = a.PrescriptionId
                })
                .FirstOrDefault();

            return appointment;
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
