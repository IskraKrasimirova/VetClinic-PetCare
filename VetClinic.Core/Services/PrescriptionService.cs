using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Prescriptions;
using VetClinic.Data;
using VetClinic.Data.Models;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Core.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly VetClinicDbContext data;

        public PrescriptionService(VetClinicDbContext data)
        {
            this.data = data;
        }

        public void Create(DateTime createdOn, string description, string appointmentId)
        {
            var appointment = this.data.Appointments
                .FirstOrDefault(a => a.Id == appointmentId);

            var prescription = new Prescription
            {
                PetId = appointment.PetId,
                Pet = appointment.Pet,
                Description = description,
                CreatedOn = createdOn,
                AppointmentId = appointmentId,
                DoctorId = appointment.DoctorId,
                Doctor = appointment.Doctor,
                Appointment = appointment,
                IsPublished = true
            };

            prescription.Id = Guid.NewGuid().ToString();
            appointment.PrescriptionId = prescription.Id;

            this.data.Prescriptions.Add(prescription);

            this.data.SaveChanges();
        }

        public PrescriptionServiceModel Details(string id)
        {
            return this.data.Prescriptions
                .Where(p => p.Id == id)
                .Select(p => new PrescriptionServiceModel
                {
                    Id = p.Id,
                    PetId = p.PetId,
                    PetName = p.Pet.Name,
                    Description = p.Description,
                    CreatedOn = p.CreatedOn.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    DoctorId= p.DoctorId,
                    DoctorFullName = p.Doctor.FullName,
                    AppointmentId = p.AppointmentId,
                    DepartmentName = p.Doctor.Department.Name,
                    ServiceName = p.Appointment.Service.Name,
                    ClientId = p.Appointment.ClientId,
                    IsPublished= true
                })
                .FirstOrDefault();
        }
    }
}
