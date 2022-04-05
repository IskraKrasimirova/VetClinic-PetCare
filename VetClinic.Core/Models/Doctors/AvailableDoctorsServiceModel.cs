namespace VetClinic.Core.Models.Doctors
{
    public class AvailableDoctorsServiceModel
    {
        public int DepartmentId { get; set; }

        public IEnumerable<DoctorServiceModel> Doctors { get; set; }
    }
}
