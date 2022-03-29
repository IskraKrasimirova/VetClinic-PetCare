using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Data;

namespace VetClinic.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly VetClinicDbContext data;

        public ClientService(VetClinicDbContext data)
        {
            this.data = data;
        }

        public string GetClientId(string userId)
        {
            return this.data.Clients
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefault();
        }
    }
}
