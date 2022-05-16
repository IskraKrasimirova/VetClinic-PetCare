using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Appointments;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;

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

            return this.View(new AppointmentsListingViewModel()
            {
                UpcomingAppointments = upcomingAppointments,
                PastAppointments = pastAppointments
            });
        }

        public IActionResult Cancel(string appointmentId)
        {
            var appointmentModel = this.appointmentService.GetAppointmentForCancel(appointmentId);

            if (appointmentModel == null)
            {
                return NotFound();
            }

            return View(appointmentModel);
        }

        [HttpPost]
        public IActionResult Delete(string appointmentId)
        {
            var isDeleted = this.appointmentService.Delete(appointmentId);

            if (!isDeleted)
            {
                this.ModelState.AddModelError(String.Empty, "Oops..Something Went Wrong");
                return View("Cancel", null);
            }

            this.TempData[GlobalMessageKey] = "Successfully delete appointment";

            return RedirectToAction("Mine", "Appointments");
        }
    }
}
