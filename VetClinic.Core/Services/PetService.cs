using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Pets;
using VetClinic.Data;
using VetClinic.Data.Enums;
using VetClinic.Data.Models;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Core.Services
{
    public class PetService : IPetService
    {
        private readonly VetClinicDbContext data;

        public PetService(VetClinicDbContext data)
        {
            this.data = data;
        }

        public AllPetsViewModel All(string petTypeName, string searchTerm, int currentPage = 1,
            int petsPerPage = int.MaxValue)
        {
            IQueryable<Pet> petsQuery = this.data.Pets.Include(p => p.PetType);

            if (!string.IsNullOrWhiteSpace(petTypeName))
            {
                petsQuery = petsQuery
                    .Where(p => p.PetType.Name == petTypeName);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                petsQuery = petsQuery.Where(p =>
                p.PetType.Name.ToLower().Contains(searchTerm.Trim().ToLower()) ||
                (p.Breed + " " + p.Name).ToLower().Contains(searchTerm.Trim().ToLower()) ||
                p.Name.ToLower().Contains(searchTerm.Trim().ToLower()) ||
                p.Breed.ToLower().Contains(searchTerm.Trim().ToLower()));
                //p.Gender.ToString().ToLower().Contains(searchTerm.Trim().ToLower()));
                //.ToList();
            }

            var totalPets = petsQuery.Count();

            var pets = GetPets(petsQuery
                .Skip((currentPage - 1) * petsPerPage)
                .Take(petsPerPage)
                .OrderBy(p => p.Name));

            var petPetTypes = AllPetTypes();

            return new AllPetsViewModel
            {
                CurrentPage = currentPage,
                TotalPets = totalPets,
                Pets = pets,
                PetTypes = petPetTypes,
                PetTypeName = petTypeName,
            };
        }

        public IEnumerable<string> AllPetTypes()
        {
            return this.data.Pets
                .Select(p => p.PetType.Name)
                .Distinct()
                .OrderBy(pt => pt)
                .ToList();
        }

        public IEnumerable<PetListingViewModel> ByClient(string clientId)
        {
            return GetPets(this.data.Pets
                .Where(p => p.ClientId == clientId));
        }

        public bool PetTypeExists(int petTypeId)
        {
            return this.data.PetTypes.Any(t => t.Id == petTypeId);
        }

        public string AddPet(
            string name,
            DateTime dateOfBirth,
            string breed,
            string gender,
            string description,
            int petTypeId,
            string clientId)
        {
            var client = this.data.Clients.Find(clientId);

            var pet = new Pet()
            {
                Name = name,
                DateOfBirth = dateOfBirth,
                Breed = breed,
                Gender = (Gender)Enum.Parse(typeof(Gender), gender),
                Description = description,
                PetTypeId = petTypeId,
                ClientId = clientId
            };

            data.Pets.Add(pet);
            client.Pets.Add(pet);

            this.data.SaveChanges();

            return pet.Id;
        }

        public bool Edit(
            string id,
            string name,
            DateTime dateOfBirth,
            string breed,
            string gender,
            string description,
            int petTypeId)
        {
            var pet = this.data.Pets.Find(id);

            if (pet == null)
            {
                return false;
            }

            pet.Name = name;
            pet.DateOfBirth = dateOfBirth;
            pet.Breed = breed;
            pet.Gender = (Gender)Enum.Parse(typeof(Gender), gender);
            pet.Description = description;
            pet.PetTypeId = petTypeId;

            this.data.SaveChanges();

            return true;
        }

        public PetDetailsServiceModel Details(string id)
        {
            return this.data.Pets
                .Where(p => p.Id == id)
                .Select(p => new PetDetailsServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    PetTypeId = p.PetTypeId,
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString(),
                    Description = p.Description,
                    ClientId = p.ClientId,
                    ClientName = p.Client.FullName,
                    UserId = p.Client.UserId
                })
                .FirstOrDefault();
        }

        public bool Delete(string id, string clientId)
        {
            var owner = this.data.Clients.Find(clientId);

            if (owner == null)
            {
                return false;
            }

            var pet = this.data.Pets
                .Where(p => p.Id == id && p.ClientId == clientId)
                .FirstOrDefault();

            if (pet == null)
            {
                return false;
            }

            owner.Pets.Remove(pet);
            this.data.Pets.Remove(pet);
            this.data.SaveChanges();

            return true;
        }

        public bool IsByOwner(string id, string clientId)
        {
            return this.data.Pets.Any(p => p.Id == id && p.ClientId == clientId);
        }

        private IEnumerable<PetListingViewModel> GetPets(IQueryable<Pet> petsQuery)
        {
            return petsQuery
                .Select(p => new PetListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString()
                })
                .ToList();
        }
    }
}
