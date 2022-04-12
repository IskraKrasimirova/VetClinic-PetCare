using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.CustomAttributes;
using VetClinic.Common;
using VetClinic.Core.Models.Services;
using VetClinic.Core.Models.Pets;

namespace VetClinic.Core.Models.Appointments
{
    public class BookAppointmentServiceModel
    {
        public BookAppointmentServiceModel()
        {
          this.WorkHours = DefaultHourSchedule.HourScheduleAsString;
        }

        [Required]
        [ValidateDateStringAttribute(ErrorMessage = "Please select a valid date!")]
        public string Date { get; set; }

        [Required]
        [ValidateHourStringAttribute(ErrorMessage = "Please select a valid hour!")]
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
