using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.Prescriptions;

namespace VetClinic.Core.Contracts
{
    public interface IPrescriptionService
    {
        void Create(
                DateTime createdOn,
                string description,
                string appointmentId);

        PrescriptionServiceModel Details(string id);
    }
}
