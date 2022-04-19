using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Prescriptions
{
    public class PrescriptionQueryModel
    {
        public string AppointmentId { get; set; }

        public string DoctorId { get; set; }
    }
}
