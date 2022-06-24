using System.ComponentModel.DataAnnotations;
using static VetClinic.Data.ModelConstants.Departmenet;

namespace VetClinic.Core.Models.Departments
{
    public class DepartmentFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Url]
        [Required]
        public string Image { get; set; }

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }
    }
}
