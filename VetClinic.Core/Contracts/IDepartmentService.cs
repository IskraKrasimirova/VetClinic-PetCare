using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Contracts
{
    public interface IDepartmentService
    {
        int Create(string name, string description);

        bool DepartmentExists(string name);
    }
}
