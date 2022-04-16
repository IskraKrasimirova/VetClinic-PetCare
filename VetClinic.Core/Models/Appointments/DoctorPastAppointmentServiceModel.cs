namespace VetClinic.Core.Models.Appointments
{
    public class DoctorPastAppointmentServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Hour { get; set; }

        public string DoctorId { get; set; }

        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string PetId { get; set; }

        public string PetName { get; set; }

        public string PetType { get; set; }

        public string ClientId { get; set; }

        public string ClientFullName { get; set; }

        public string ClientPhoneNumber { get; set; }
    }
}
