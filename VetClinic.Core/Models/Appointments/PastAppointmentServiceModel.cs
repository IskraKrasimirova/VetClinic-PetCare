namespace VetClinic.Core.Models.Appointments
{
    public class PastAppointmentServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Hour { get; set; }

        public string DoctorId { get; set; }

        public string DoctorFullName { get; set; }

        public string DoctorPhoneNumber { get; set; }

        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string ClientId { get; set; }

        public string PetId { get; set; }

        public string PetName { get; set; }
    }
}
