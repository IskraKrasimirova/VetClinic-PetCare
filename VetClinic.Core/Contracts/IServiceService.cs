using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Contracts
{
    public interface IServiceService
    {
        //IEnumerable<ServiceViewModel> GetAllServices();

        AllServicesViewModel All(string departmentName, string searchTerm);

        AvailableServicesViewModel ByDepartment(AvailableServicesViewModel query);
    }
}
