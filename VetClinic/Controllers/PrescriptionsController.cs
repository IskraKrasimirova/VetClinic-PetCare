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
        private readonly IPetService petService;

        public PrescriptionsController(
            IPrescriptionService prescriptionService, 
            IAppointmentService appointmentService,
            IDoctorService doctorService, 
            IClientService clientService, 
            IPetService petService)
        {
            this.prescriptionService = prescriptionService;
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.clientService = clientService;
            this.petService = petService;
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

            var prescriptionId = appointment.PrescriptionId;

            if (prescriptionId != null)
            {
                var existingPrescription = this.prescriptionService.Details(prescriptionId);
                this.TempData[GlobalMessageKey] = "The prescription already exists.";
                return View("Details", existingPrescription);
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
            var appointmentId = prescription.AppointmentId;

            if (appointmentId == null)
            {
                return NotFound();
            }

            var appointment = this.appointmentService.GetPastAppointment(appointmentId);
            
            if (DateTime.Compare(prescription.CreatedOn, appointment.Date) < 0) 
            {
                this.ModelState.AddModelError(String.Empty, "The prescription date must be after the appointment day.");
                return View(prescription);
            }

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

            return View(prescription);
        }

        [Authorize(Roles = $"{ClientRoleName}, {DoctorRoleName}")]
        public IActionResult ByPet(string petId)
        {
            if (petId == null)
            {
                return BadRequest();
            }

            if (!this.User.IsClient() && !this.User.IsDoctor())
            {
                return Unauthorized();
            }

            var petPrescriptions = this.prescriptionService.GetPrescriptionsByPet(petId);

            var userId = this.User.GetId();

            if (this.User.IsClient())
            {
                var clientId = this.clientService.GetClientId(userId);

                if (clientId == null)
                {
                    return BadRequest();
                }

                if (petPrescriptions.Any(p => p.ClientId != clientId))
                {
                    return Unauthorized();
                }
            }

            var pet = this.petService.GetPet(petId);
            ViewBag.Pet = $"{pet.Name} - {pet.PetType}";

            return View(petPrescriptions);
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult All()
        {
            if (!this.User.IsDoctor())
            {
                return Unauthorized();
            }

            var userId = this.User.GetId();

            var doctor = doctorService.GetDoctor(userId);

            if (doctor.Id == null)
            {
                return BadRequest();
            }

            var prescriptions = this.prescriptionService.GetMine(doctor.Id);

            return View(prescriptions);
        }
    }
}
