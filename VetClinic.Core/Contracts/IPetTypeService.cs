using VetClinic.Core.Models.PetTypes;

namespace VetClinic.Core.Contracts
{
    public interface IPetTypeService
    {
        IEnumerable<PetTypeServiceModel> GetPetTypes();
    }
}
