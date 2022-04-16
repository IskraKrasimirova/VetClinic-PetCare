namespace VetClinic.Core.Contracts
{
    public interface IHomeService
    {
        string GetDoctorFullName(string userId);

        string GetClientFullName(string userId);

        string GetAdminFullName(string userId);
    }
}
