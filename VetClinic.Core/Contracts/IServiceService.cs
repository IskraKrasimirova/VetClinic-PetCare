using VetClinic.Core.Models.Services;

namespace VetClinic.Core.Contracts
{
    public interface IServiceService
    {
        //IEnumerable<ServiceViewModel> GetAllServices();

        AllServicesViewModel All(string departmentName, string searchTerm);

        AvailableServicesViewModel ByDepartment(AvailableServicesViewModel query);

        bool ServiceExists(string name);

        int Create(string name, string description, decimal price, int departmentId);

        ServiceViewModel Details(int id);

        bool Edit(int id, string name, string description, decimal price, int departmentId);

        bool Delete(int id);

        IEnumerable<ServiceViewModel> AllServices();
    }
}
