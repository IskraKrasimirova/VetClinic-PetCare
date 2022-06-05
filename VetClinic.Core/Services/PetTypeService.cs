using VetClinic.Core.Contracts;
using VetClinic.Core.Models.PetTypes;
using VetClinic.Data;

namespace VetClinic.Core.Services
{
    public class PetTypeService : IPetTypeService
    {
        private readonly VetClinicDbContext data;

        public PetTypeService(VetClinicDbContext data)
        {
            this.data = data;
        }

        public IEnumerable<PetTypeServiceModel> GetPetTypes()
        {
            return this.data.PetTypes
                .Select(p => new PetTypeServiceModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }
    }
}
