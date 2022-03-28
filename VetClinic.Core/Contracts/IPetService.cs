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

        string GetClientId(string userId);

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

        bool IsByOwner(string id, string clientId);
    }
}
