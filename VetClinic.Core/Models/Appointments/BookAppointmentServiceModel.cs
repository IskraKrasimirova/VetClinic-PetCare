using System.ComponentModel.DataAnnotations;
using VetClinic.Common;
using VetClinic.Core.CustomAttributes;
using VetClinic.Core.Models.Pets;
using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Models.Appointments
{
    public class BookAppointmentServiceModel
    {
        public BookAppointmentServiceModel()
        {
          this.WorkHours = DefaultHourSchedule.HourScheduleAsString;
        }

        [Required]
        [ValidateDateStringAttribute(ErrorMessage = "Please, select a valid date!")]
        public string Date { get; set; }

        [Required]
        [ValidateHourStringAttribute(ErrorMessage = "Please, select a valid hour!")]
        public string Hour { get; set; }

        [Required]
        public string DoctorId { get; set; }

        public string DoctorFullName { get; set; }

        public string ClientId { get; set; }

        public int ServiceId { get; set; }

        public string PetId { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }

        public IEnumerable<PetListingViewModel> Pets { get; set; }

        public IEnumerable<string> WorkHours { get; set; } 
    }
}
