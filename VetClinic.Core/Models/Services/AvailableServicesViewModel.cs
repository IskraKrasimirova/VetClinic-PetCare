namespace VetClinic.Core.Models.Services
{
    public class AvailableServicesViewModel
    {
        public int DepartmentId { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
    }
}
