using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Contracts;
using VetClinic.Data;

namespace VetClinic.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly VetClinicDbContext data;

        public DoctorService(VetClinicDbContext data)
        {
            this.data = data;
        }


    }
}
