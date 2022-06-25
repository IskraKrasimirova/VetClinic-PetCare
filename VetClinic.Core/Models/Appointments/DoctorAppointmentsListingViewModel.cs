namespace VetClinic.Core.Models.Appointments
{
    public class DoctorAppointmentsListingViewModel
    {
        public IEnumerable<DoctorUpcomingAppointmentServiceModel> UpcomingAppointments { get; set; }

        public IEnumerable<DoctorPastAppointmentServiceModel> PastAppointments { get; set; }
    }
}
