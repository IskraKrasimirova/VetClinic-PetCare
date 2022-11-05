using Microsoft.AspNetCore.Mvc;
using System.Text;
using VetClinic.Core.Contracts;
using VetClinic.Core.Messaging;
using VetClinic.Core.Models.Appointments;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Areas.Doctor.Controllers
{
    public class AppointmentsController : DoctorsController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IEmailSender emailSender;

        public AppointmentsController(IAppointmentService appointmentService, IEmailSender emailSender)
        {
            this.appointmentService = appointmentService;
            this.emailSender = emailSender;
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

        [HttpPost]
        public async Task<IActionResult> SendEmail(string appointmentId)
        {
            var appointment = this.appointmentService.GetAppointmentForCancel(appointmentId);
            var html = new StringBuilder();
            html.AppendLine($"<h1>Dear Mr/Mrs {appointment.ClientFullName},</h1>");
            html.AppendLine($"<h3>We are sorry to let you know that your appointment is canceled.</h3>");
            html.AppendLine($"<h3>Canceled appointment information:</h3>");
            html.AppendLine($"<h3>Date: {appointment.Date.ToString(DateFormat)} {appointment.Hour}</h3>");
            html.AppendLine($"<h3>Pet: {appointment.PetName}</h3>");
            html.AppendLine($"<h3>Service: {appointment.ServiceName}</h3>");
            html.AppendLine($"<h3>Doctor: {appointment.DoctorFullName}</h3>");
            html.AppendLine($"<h3>Please, visit our site to make another appointment or call us for additional information.</h3>");
            html.AppendLine($"<h3>We apologize for the caused inconvenience.</h3>");
            html.AppendLine();
            html.AppendLine($"<h3>Best regards,</h3>");
            html.AppendLine($"<h3>Dr {appointment.DoctorFullName},</h3>");
            html.AppendLine($"<h3>Vet Clinic PetCare</h3>");
            html.AppendLine($"<h3>Phone Number: +359 888 123456</h3>");
            html.AppendLine($"<h3>Email: vetclinic@petcare.com</h3>");
            //await this.emailSender.SendEmailAsync("vetclinic@petcare.com", "Vet Clinic PetCare", $"{appointment.ClientEmail}", "Canceled appointment", html.ToString());
            await this.emailSender.SendEmailAsync("iskra_krasimirova@abv.bg", "Vet Clinic PetCare", $"{appointment.ClientEmail}", "Canceled appointment", html.ToString()); 
            return this.RedirectToAction("Cancel", new { appointmentId });
        }
    }
}
