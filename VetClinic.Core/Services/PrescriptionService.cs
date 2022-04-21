using System.Globalization;
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

            if (appointment.PrescriptionId == null)
            {
                var prescription = new Prescription
                {
                    Id = Guid.NewGuid().ToString(),//Ако не подам - гърми!!!
                    PetId = appointment.PetId,
                    Pet = appointment.Pet,
                    Description = description,
                    CreatedOn = createdOn,
                    AppointmentId = appointmentId,
                    DoctorId = appointment.DoctorId,
                    Doctor = appointment.Doctor,
                    Appointment = appointment
                };

                this.data.Prescriptions.Add(prescription);
                appointment.PrescriptionId = prescription.Id;

                this.data.SaveChanges();
            }
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
                    DoctorId = p.DoctorId,
                    DoctorFullName = p.Doctor.FullName,
                    AppointmentId = p.AppointmentId,
                    DepartmentName = p.Doctor.Department.Name,
                    ServiceName = p.Appointment.Service.Name,
                    ClientId = p.Appointment.ClientId
                })
                .FirstOrDefault();
        }

        public IEnumerable<PrescriptionServiceModel> GetMine(string doctorId)
        {
            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
            {
                return null;
            }

            var doctorPrescriptions = this.data.Prescriptions
                .Where(p => p.DoctorId == doctorId)
                .OrderByDescending(p => p.CreatedOn)
                .ThenBy(p => p.Pet.Name)
                .Select(p => new PrescriptionServiceModel
                {
                    Id = p.Id,
                    PetId = p.PetId,
                    PetName = p.Pet.Name,
                    Description = p.Description,
                    CreatedOn = p.CreatedOn.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    DoctorId = p.DoctorId,
                    DoctorFullName = p.Doctor.FullName,
                    AppointmentId = p.AppointmentId,
                    DepartmentName = p.Doctor.Department.Name,
                    ServiceName = p.Appointment.Service.Name,
                    ClientId = p.Appointment.ClientId
                })
                .ToList();

            return doctorPrescriptions;
        }

        public IEnumerable<PrescriptionServiceModel> GetPrescriptionsByPet(string petId)
        {
            var pet = this.data.Pets
                .FirstOrDefault(p => p.Id == petId);

            if (pet == null)
            {
                return null;
            }

            var petPrescriptions = this.data.Prescriptions
                .Where(p => p.PetId == petId)
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new PrescriptionServiceModel
                {
                    Id = p.Id,
                    PetId = p.PetId,
                    PetName = p.Pet.Name,
                    Description = p.Description,
                    CreatedOn = p.CreatedOn.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    DoctorId = p.DoctorId,
                    DoctorFullName = p.Doctor.FullName,
                    AppointmentId = p.AppointmentId,
                    DepartmentName = p.Doctor.Department.Name,
                    ServiceName = p.Appointment.Service.Name,
                    ClientId = p.Appointment.ClientId
                })
                .ToList();

            return petPrescriptions;
        }
    }
}
