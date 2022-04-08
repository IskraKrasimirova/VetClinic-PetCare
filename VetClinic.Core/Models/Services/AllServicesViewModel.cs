using System.ComponentModel.DataAnnotations;

namespace VetClinic.Core.Models.Services
{
    public class AllServicesViewModel
    {
        public int DepartmentId { get; set; }

        public string Department { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public IEnumerable<string> Departments { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
    }
}
