using VetClinic.Core.Models.Departments;

namespace VetClinic.Core.Contracts
{
    public interface IDepartmentService
    {
        int Create(string name, string image, string description);

        bool DepartmentExists(string name);

        IEnumerable<string> AllDepartments();

        IEnumerable<DepartmentListingViewModel> GetAllDepartments();

        DepartmentDetailsServiceModel Details(int id);

        bool Edit(int id, string name, string image, string description);

        bool Delete(int id);
    }
}
