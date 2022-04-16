using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Appointments;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Controllers
{
    [Authorize(Roles = ClientRoleName)]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IClientService clientService;
        private readonly IPetService petService;

        public AppointmentsController(IAppointmentService appointmentService, IClientService clientService, IPetService petService)
        {
            this.appointmentService = appointmentService;
            this.clientService = clientService;
            this.petService = petService;
        }

        public IActionResult Book([FromQuery] AppointmentQueryModel query)
        {
            var appointment = this.appointmentService.GetDoctorSchedule(query.DoctorId);

            if (appointment == null)
            {
                return BadRequest();
            }

            var clientId = clientService.GetClientId(this.User.GetId());

            if (clientId == null)
            {
                return BadRequest();
            }

            var myPets = this.petService.ByClient(clientId);

            var doctorId = query.DoctorId;
            appointment.DoctorId = doctorId;
            appointment.ClientId = clientId;
            appointment.Services = this.appointmentService.AllServices(doctorId);
            appointment.Pets = myPets;

            return View(appointment);
        }

        [HttpPost]
        public IActionResult Book(BookAppointmentServiceModel query)
        {
            var doctorId = query.DoctorId;
            var serviceId = query.ServiceId;
            var clientId = clientService.GetClientId(this.User.GetId());
            var petId = query.PetId;

            if (!this.ModelState.IsValid)
            {
                query.Services = this.appointmentService.AllServices(doctorId);
                query.Pets = this.petService.ByClient(clientId);

                return View(query);
            }
                
            var appointmentHourAsString = query.Hour.Trim();
            var appointmentDateTime = this.appointmentService
                .TryToParseDate(query.Date, appointmentHourAsString);

            var availableHours = this.appointmentService
                    .CheckDoctorAvailableHours(appointmentDateTime, appointmentHourAsString, doctorId);

            if (availableHours != null)
            {
                this.ModelState.AddModelError(string.Empty, availableHours);
                query.Services = this.appointmentService.AllServices(doctorId);
                query.Pets = this.petService.ByClient(clientId);
                return View(query);
            }

            this.appointmentService.AddNewAppointment
                (clientId, doctorId, serviceId, petId, appointmentDateTime, appointmentHourAsString);

            this.TempData[GlobalMessageKey] = "Successfully booked an appointment!";

            return RedirectToAction("Mine", "Appointments");
        }

        public IActionResult Mine()
        {
            var clientId = clientService.GetClientId(this.User.GetId());

            if (clientId == null)
            {
                return BadRequest();
            }

            var upcomingAppointments = this.appointmentService
                .GetUpcomingAppointments(clientId);

            var pastAppointments = this.appointmentService
                .GetPastAppointments(clientId);

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
