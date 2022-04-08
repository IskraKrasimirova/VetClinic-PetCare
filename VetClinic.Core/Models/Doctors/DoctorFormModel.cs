using System.ComponentModel.DataAnnotations;
using VetClinic.Core.Models.Departments;
using static VetClinic.Data.ModelConstants.Doctor;


namespace VetClinic.Core.Models.Doctors
{
    public class DoctorFormModel
    {
        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [RegularExpression(PhoneNumberRegex)]
        public string PhoneNumber { get; set; }

        [Required]
        [Url]
        [Display(Name = "Profile Image")]
        public string ProfileImage { get; set; }

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public IEnumerable<DepartmentListingViewModel>? Departments { get; set; }
    }
}
