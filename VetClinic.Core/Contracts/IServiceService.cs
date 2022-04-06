using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Contracts
{
    public interface IServiceService
    {
        //IEnumerable<ServiceViewModel> GetAllServices();

        AllServicesViewModel All(string departmentName);
    }
}
