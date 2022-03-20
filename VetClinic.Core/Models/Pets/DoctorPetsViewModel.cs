using System.ComponentModel.DataAnnotations;
using VetClinic.Data.Models;

namespace VetClinic.Core.Models.Pets
{
    public class DoctorPetsViewModel
    {
        [Required]
        public string PetId { get; set; }
        public Pet Pet { get; set; }

        [Required]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
