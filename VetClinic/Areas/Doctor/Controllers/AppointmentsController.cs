using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Appointments;
using VetClinic.Extensions;

namespace VetClinic.Areas.Doctor.Controllers
{
    public class AppointmentsController : DoctorsController
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public IActionResult Mine()
        {
            var userId = this.User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            var upcomingAppointments = this.appointmentService
                .GetDoctorUpcomingAppointments(userId);

            var pastAppointments = this.appointmentService
                .GetDoctorPastAppointments(userId);

            return this.View(new DoctorAppointmentsListingViewModel()
            {
                UpcomingAppointments = upcomingAppointments,
                PastAppointments = pastAppointments
            });
        }
    }
}
