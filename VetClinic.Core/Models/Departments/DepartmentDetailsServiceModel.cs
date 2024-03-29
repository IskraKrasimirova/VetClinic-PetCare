﻿using VetClinic.Core.Models.Doctors;
using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Models.Departments
{
    public class DepartmentDetailsServiceModel : DepartmentListingViewModel
    {
        public IEnumerable<DoctorServiceModel> Doctors { get; set; }

        public IEnumerable<AllServicesViewModel> Services { get; set; }
    }
}
