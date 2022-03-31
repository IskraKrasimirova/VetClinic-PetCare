using VetClinic.Core.Models.Departments;

namespace VetClinic.Core.Contracts
{
    public interface IDepartmentService
    {
        int Create(string name, string image, string description);

        bool DepartmentExists(string name);

        IEnumerable<DepartmentListingViewModel> GetAllDepartments();
    }
}
