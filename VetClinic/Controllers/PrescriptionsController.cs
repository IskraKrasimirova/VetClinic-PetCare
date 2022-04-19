using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Prescriptions;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly IPrescriptionService prescriptionService;
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IClientService clientService;

        public PrescriptionsController(IPrescriptionService prescriptionService, IAppointmentService appointmentService,
            IDoctorService doctorService, IClientService clientService)
        {
            this.prescriptionService = prescriptionService;
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.clientService = clientService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult Add([FromQuery] PrescriptionQueryModel query)
        {
            if (!this.User.IsDoctor())
            {
                return Unauthorized();
            }

            var userId = this.User.GetId();

            var doctor = doctorService.GetDoctor(userId);

            if (doctor.Id != query.DoctorId)
            {
                return Unauthorized();
            }

            var appointmentId = query.AppointmentId;
            var doctorId = query.DoctorId;

            if (appointmentId == null || doctorId == null)
            {
                return BadRequest();
            }

            var appointment = this.appointmentService.GetPastAppointment(appointmentId);

            if (appointment == null)
            {
                return BadRequest();
            }

            return View(new PrescriptionFormModel
            {
                PetId = appointment.PetId,
                PetName = appointment.PetName,
                DoctorId = appointment.DoctorId,
                DoctorFullName = doctor.FullName,
                AppointmentId = appointmentId,
                ServiceName = appointment.ServiceName,
                DepartmentName = doctor.Department,
            });
        }

        [HttpPost]
        [Authorize(Roles = DoctorRoleName)]
        public IActionResult Add(PrescriptionFormModel prescription)
        {
            if (!ModelState.IsValid)
            {
                return View(prescription);
            }

            /*var prescriptionId = */
            this.prescriptionService.Create(
                prescription.CreatedOn,
                prescription.Description,
                prescription.AppointmentId);

            this.TempData[GlobalMessageKey] = "Successfully make a prescription!";

            return RedirectToAction("Mine", "Appointments", new { Area = "Doctor" });
           // return RedirectToAction("Details", new { id = prescriptionId });
        }

        [Authorize(Roles = $"{ClientRoleName}, {DoctorRoleName}")]
        public IActionResult Details(string appointmentId)
        {
            if (!this.User.IsClient() && !this.User.IsDoctor())
            {
                return Unauthorized();
            }
            var appointment = this.appointmentService.GetPastAppointment(appointmentId);

            if (appointment == null)
            {
                return BadRequest();
            }

            var prescription = this.prescriptionService.Details(appointment.PrescriptionId);

            if (prescription == null)
            {
                return NotFound();
            }

            var userId = this.User.GetId();

            if (this.User.IsClient())
            {
                var clientId = this.clientService.GetClientId(userId);

                if (clientId == null)
                {
                    return BadRequest();
                }

                if (prescription.ClientId != clientId)
                {
                    return Unauthorized();
                }
            }
            else //User is doctor
            {
                var doctor = doctorService.GetDoctor(userId);

                if (prescription.DoctorId != doctor.Id)
                {
                    return Unauthorized();
                }
            }

            return View(prescription);
        }
    }
}
