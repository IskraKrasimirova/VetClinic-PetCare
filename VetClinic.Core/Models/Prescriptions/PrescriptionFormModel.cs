using System.ComponentModel.DataAnnotations;
using static VetClinic.Data.ModelConstants.Prescription;

namespace VetClinic.Core.Models.Prescriptions
{
    public class PrescriptionFormModel
    {
        [Required]
        public string PetId { get; set; }

        [Required]
        public string PetName { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [Required]
        public string DoctorFullName { get; set; }

        [Required]
        public string AppointmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string ServiceName { get; set; }
    }
}
