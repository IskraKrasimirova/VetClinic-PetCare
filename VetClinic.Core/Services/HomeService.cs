using VetClinic.Core.Contracts;
using VetClinic.Data;

namespace VetClinic.Core.Services
{
    public class HomeService : IHomeService
    {
        private readonly VetClinicDbContext data;

        public HomeService(VetClinicDbContext data)
        {
            this.data = data;
        }

        public string GetAdminFullName(string userId)
        {
            var admin = this.data.Users
                .FirstOrDefault(u => u.Id == userId);

            if (admin == null)
            {
                return null;
            }

            return admin.FullName;
        }

        public string GetClientFullName(string userId)
        {
            var client = this.data.Clients
                .FirstOrDefault(c => c.UserId == userId);

            if (client == null)
            {
                return null;
            }

            return client.FullName;
        }

        public string GetDoctorFullName(string userId)
        {
            var doctor = this.data.Doctors
                .FirstOrDefault(d => d.UserId == userId);

            if (doctor == null)
            {
                return null;
            }

            return doctor.FullName;
        }
    }
}
