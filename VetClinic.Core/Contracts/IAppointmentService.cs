﻿using VetClinic.Core.Models.Appointments;
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

        CancelAppointmentServiceModel GetAppointmentForCancel(string appointmentId);

        bool Delete(string appointmentId);

        IEnumerable<UpcomingAppointmentServiceModel> GetDoctorUpcomingAppointments(string userId);

        IEnumerable<PastAppointmentServiceModel> GetDoctorPastAppointments(string userId);

        PastAppointmentServiceModel GetPastAppointment(string appointmentId);

        bool CheckPetAppointmentsAtTheSameDateAndHour(DateTime appointmentDateTime, string appointmentHourAsString, string petId, string clientId);
    }
}
