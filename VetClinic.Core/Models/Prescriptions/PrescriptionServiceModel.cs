namespace VetClinic.Core.Models.Prescriptions
{
    public class PrescriptionServiceModel
    {
        public string Id { get; set; }

        public string PetId { get; set; }

        public string PetName { get; set; }

        public string ClientId { get; set; }

        public string Description { get; set; }

        public string CreatedOn { get; set; }

        public string DoctorId { get; set; }

        public string DoctorFullName { get; set; }

        public string AppointmentId { get; set; }

        public string DepartmentName { get; set; }

        public string ServiceName { get; set; }
    }
}
