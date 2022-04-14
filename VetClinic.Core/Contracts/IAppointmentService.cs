using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.Appointments;
using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Contracts
{
    public interface IAppointmentService
    {
        BookAppointmentServiceModel GetDoctorSchedule(string doctorId);

        IEnumerable<ServiceViewModel> AllServices(string doctorId);

        DateTime TryToParseDate(string dateAsString, string hourAsString);

        string CheckDoctorAvailableHours(DateTime appointmentDateTime, string hourAsString, string doctorId);

        void AddNewAppointment(
             string clientId,
             string doctorId,
             int serviceId,
             string petId,
             DateTime appointmentDateTime,
             string hourAsString);

        IEnumerable<UpcomingAppointmentServiceModel> GetUpcomingAppointments(string clientId);
        IEnumerable<PastAppointmentServiceModel> GetPastAppointments(string clientId);
    }
}
