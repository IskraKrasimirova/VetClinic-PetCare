namespace VetClinic.Core.Models.Appointments
{
    public class CancelAppointmentServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Hour { get; set; }

        public string PetName { get; set; }

        public string DoctorFullName { get; set; }

        public string ServiceName { get; set; }
    }
}
