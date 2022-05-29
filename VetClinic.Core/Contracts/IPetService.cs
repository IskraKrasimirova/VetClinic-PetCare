using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Models.Pets;

namespace VetClinic.Core.Contracts
{
    public interface IPetService
    {
        AllPetsViewModel All(string petTypeName, string searchTerm, int currentPage = 1,
    int petsPerPage = int.MaxValue);

        IEnumerable<string> AllPetTypes();

        IEnumerable<PetListingViewModel> ByClient(string clientId);

        bool PetTypeExists(int petTypeId);

        string AddPet(
            string name,
            DateTime dateOfBirth,
            string breed,
            string gender,
            string description,
            int petTypeId,
            string clientId);

        bool Edit(
            string id,
            string name,
            DateTime dateOfBirth,
            string breed,
            string gender,
            string description,
            int petTypeId);

        PetDetailsServiceModel Details(string id);

        bool Delete(string id, string clientId);

        bool IsByOwner(string id, string clientId);

        PetServiceModel GetPet(string id);

        PetDeleteServiceModel GetPetForDelete(string id);
    }
}
