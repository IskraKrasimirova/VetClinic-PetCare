using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Services
{
    public class AvailableServicesViewModel
    {
        public int DepartmentId { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
    }
}
