using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Data.Models
{
    public class DoctorService
    {
        [Required]
        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
