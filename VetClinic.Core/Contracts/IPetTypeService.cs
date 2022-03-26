using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.PetTypes;

namespace VetClinic.Core.Contracts
{
    public interface IPetTypeService
    {
        IEnumerable<PetTypeServiceModel> GetPetTypes();
    }
}
