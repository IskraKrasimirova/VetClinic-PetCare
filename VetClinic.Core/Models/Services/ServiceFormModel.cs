using System.ComponentModel.DataAnnotations;
using VetClinic.Core.Models.Departments;
using static VetClinic.Data.ModelConstants.Service;

namespace VetClinic.Core.Models.Services
{
    public class ServiceFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public IEnumerable<DepartmentListingViewModel> Departments { get; set; }

        
    }
}
