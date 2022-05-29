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
                p.Name.ToLower().Contains(searchTerm.Trim().ToLower()) ||
                p.Breed.ToLower().Contains(searchTerm.Trim().ToLower()));
                //p.Gender.ToString().ToLower().Contains(searchTerm.Trim().ToLower()));
                //.ToList();
            }

            petsQuery = petsQuery
                .OrderBy(p => p.PetType.Name)
                .ThenBy(p => p.Name)
                .ThenBy(p => p.Breed);

            var totalPets = petsQuery.Count();

            var pets = GetPets(petsQuery
                .Skip((currentPage - 1) * petsPerPage)
                .Take(petsPerPage));

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
                .OrderBy(p => p)
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
                    DateOfBirth = p.DateOfBirth.ToString(DateFormat, CultureInfo.InvariantCulture),
                    PetTypeId = p.PetTypeId,
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString(),
                    Description = p.Description,
                    ClientId = p.ClientId,
                    ClientName = p.Client.FullName,
                    ClientPhoneNumber = this.data.Users
                                        .FirstOrDefault(u => u.FullName == p.Client.FullName)
                                        .PhoneNumber,
                    UserId = p.Client.UserId
                })
                .FirstOrDefault();
        }

        public PetDeleteServiceModel GetPetForDelete(string id)
        {
            return this.data.Pets
                .Where(p => p.Id == id)
                .Select(p => new PetDeleteServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth.ToString(DateFormat, CultureInfo.InvariantCulture),
                    PetTypeId = p.PetTypeId,
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString(),
                    Description = p.Description,
                    ClientId = p.ClientId,
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
                .Include(p => p.Appointments)
                .Include(p => p.Prescriptions)
                .FirstOrDefault();

            if (pet == null)
            {
                return false;
            }

            var prescriptions = pet.Prescriptions.ToArray();
            this.data.Prescriptions.RemoveRange(prescriptions);
            this.data.SaveChanges();

            var appointments = pet.Appointments.ToArray();
            this.data.Appointments.RemoveRange(appointments);
            this.data.SaveChanges();

            var doctors = this.data.PetDoctors
                .Where(p => p.PetId == id)
                .ToArray();
            this.data.PetDoctors.RemoveRange(doctors);
            this.data.SaveChanges();

            var services = this.data.PetServices
                .Where(p => p.PetId == id)
                .ToArray();
            this.data.PetServices.RemoveRange(services);
            this.data.SaveChanges();

            owner.Pets.Remove(pet);
            this.data.Pets.Remove(pet);
            this.data.SaveChanges();

            var petType = this.data.PetTypes
                .FirstOrDefault(pt => pt.Id == pet.PetTypeId);
            petType.Pets.Remove(pet);
            this.data.SaveChanges();

            return true;
        }

        public bool IsByOwner(string id, string clientId)
        {
            return this.data.Pets.Any(p => p.Id == id && p.ClientId == clientId);
        }

        public PetServiceModel GetPet(string id)
        {
            return this.data.Pets
                .Where(p => p.Id == id)
                .Select(p => new PetServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Breed = p.Breed,
                    PetType = p.PetType.Name
                })
                .FirstOrDefault();
        }

        private IEnumerable<PetListingViewModel> GetPets(IQueryable<Pet> petsQuery)
        {
            return petsQuery
                .Select(p => new PetListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth.ToString(DateFormat, CultureInfo.InvariantCulture),
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString()
                })
                .OrderBy(p => p.PetType)
                .ThenBy(p => p.Breed)
                .ThenBy(p => p.Name)
                .ToList();
        }
    }
}
