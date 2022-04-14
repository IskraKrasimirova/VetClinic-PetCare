namespace VetClinic.Core.Models.Appointments
{
    public class AppointmentsListingViewModel
    {
        public IEnumerable<UpcomingAppointmentServiceModel> UpcomingAppointments { get; set; }

        public IEnumerable<PastAppointmentServiceModel> PastAppointments { get; set; }
    }
}
