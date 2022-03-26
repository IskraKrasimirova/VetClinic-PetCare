using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var petsQuery = this.data.Pets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(petTypeName))
            {
                petsQuery = petsQuery.Where(p => p.PetType.Name == petTypeName);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                petsQuery = petsQuery.Where(p =>
                p.PetType.Name.ToLower().Contains(searchTerm.ToLower()) ||
                (p.PetType.Name + " " + p.Breed).ToLower().Contains(searchTerm.ToLower()) ||
                p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalPets = petsQuery.Count();

            var pets = petsQuery
                .Skip((currentPage - 1) * AllPetsViewModel.PetsPerPage)
                .Take(AllPetsViewModel.PetsPerPage)
                .OrderBy(p => p.Name)
                .Select(p => new PetListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    //p.DateOfBirth,
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString()
                })
                .ToList();

            var petPetTypes = AllPetTypes();

            return new AllPetsViewModel
            {
                CurrentPage = currentPage,
                TotalPets = totalPets,
                Pets = pets,
                PetTypes = petPetTypes,
                PetTypeName = petTypeName,
                SearchTerm = searchTerm
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
            return this.data.Pets
                .Where(p => p.ClientId == clientId)
                .Select(p => new PetListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth.ToString(NormalDateFormat, CultureInfo.InvariantCulture),
                    //DateTime.ParseExact(
                    //            p.DateOfBirth.ToString(),
                    //            NormalDateFormat,
                    //            CultureInfo.InvariantCulture),
                    //p.DateOfBirth,
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString()
                })
                .ToList();
        }

        public string GetClientId(string userId)
        {
            return this.data.Clients
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefault();
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
                //DateTime.ParseExact(
                //            pet.DateOfBirth.ToString(),
                //            NormalDateFormat,
                //            CultureInfo.InvariantCulture),
                Breed = breed,
                Gender = (Gender)Enum.Parse(typeof(Gender) ,gender),
                Description = description,
                PetTypeId = petTypeId,
                ClientId = clientId
            };

            data.Pets.Add(pet);
            client.Pets.Add(pet);

            this.data.SaveChanges();

            return pet.Id;
        }
    }
}
