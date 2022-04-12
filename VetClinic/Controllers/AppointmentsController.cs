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
        private readonly IServiceService serviceService;
        private readonly IPetService petService;

        public AppointmentsController(IAppointmentService appointmentService, IClientService clientService, IServiceService serviceService, IPetService petService)
        {
            this.appointmentService = appointmentService;
            this.clientService = clientService;
            this.serviceService = serviceService;
            this.petService = petService;
        }
        public IActionResult Index()
        {
            return View();
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
                //return RedirectToAction("Book", new { doctorId, serviceId, clientId, petId });
            }
                
            var appointmentHourAsString = query.Hour.Trim();
            var appointmentDateTime = this.appointmentService
                .TryToParseDate(query.Date, appointmentHourAsString);

            var availableHours = this.appointmentService
                    .CheckDoctorAvailableHours(appointmentDateTime, appointmentHourAsString, doctorId);

            if (availableHours != null)
            {
                return View(query);
            }

            this.appointmentService.AddNewAppointment
                (clientId, doctorId, serviceId, petId, appointmentDateTime, appointmentHourAsString);

            return RedirectToAction("Index", "Home");
        }
    }
}
