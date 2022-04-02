using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Doctors
{
    public class DoctorServiceModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ProfileImage { get; set; }

        public int DepartmentId { get; set; }

        public string Department { get; set; }
    }
}
