using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.Doctors;
using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Models.Departments
{
    public class DepartmentDetailsServiceModel : DepartmentListingViewModel
    {
        public IEnumerable<DoctorServiceModel> Doctors { get; set; }

        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
