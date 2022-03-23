using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Extensions;
using VetClinic.Core.Models.Pets;
using static VetClinic.Common.GlobalConstants;
using static VetClinic.Common.GlobalConstants.FormattingConstants;
using System.Globalization;

namespace VetClinic.Controllers
{
    public class PetsController : Controller
    {
        private readonly VetClinicDbContext data;

        public PetsController(VetClinicDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!UserIsClient())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new PetFormModel
            {
                PetTypes = GetPetTypes()
            });
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult All([FromQuery]AllPetsViewModel query)
        {
            var petsQuery = this.data.Pets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.PetTypeName))
            {
                petsQuery = petsQuery.Where(p => p.PetType.Name == query.PetTypeName);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                petsQuery = petsQuery.Where(p => 
                p.PetType.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                (p.PetType.Name + " " + p.Breed).ToLower().Contains(query.SearchTerm.ToLower()) ||
                p.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            var totalPets = petsQuery.Count();

            var pets = petsQuery
                .Skip((query.CurrentPage -1)*AllPetsViewModel.PetsPerPage)
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

            var petPetTypes = this.data.Pets
                .Select(p => p.PetType.Name)
                .Distinct()
                .OrderBy(pt => pt)
                .ToList();

            query.TotalPets = totalPets;
            query.Pets = pets;  
            query.PetTypes = petPetTypes;   

            return View(query);
        }

        [Authorize(Roles = ClientRoleName)]
        public IActionResult Mine()
        {
            var clientId = this.data.Clients
                .Where(c => c.UserId == this.User.GetId())
                .Select(c => c.Id)
                .FirstOrDefault();

            if (clientId == null)
            {
                return BadRequest();
            }

            var myPets = this.data.Pets
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

            return View(myPets);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(PetFormModel pet)
        {
            var clientId = this.data.Clients
                .Where(c => c.UserId == this.User.GetId())
                .Select(c => c.Id)
                .FirstOrDefault();

            var client = this.data.Clients.Find(clientId);

            if (!data.PetTypes.Any(t => t.Id == pet.PetTypeId))
            {
                this.ModelState.AddModelError(nameof(pet.PetTypeId), "Pet type does not exist.");
            }

            if (!ModelState.IsValid)
            {
                pet.PetTypes = GetPetTypes();

                return View(pet);
            }

            var petData = new Pet()
            {
                Name = pet.Name,
                DateOfBirth = pet.DateOfBirth,
                //DateTime.ParseExact(
                //            pet.DateOfBirth.ToString(),
                //            NormalDateFormat,
                //            CultureInfo.InvariantCulture),
                Breed = pet.Breed,
                Gender = pet.Gender,
                Description = pet.Description,
                PetTypeId = pet.PetTypeId,
                ClientId = clientId 
            };

            data.Pets.Add(petData);
            client.Pets.Add(petData);

            //if (this.UserIsDoctor())
            //{
            //    var doctorId = this.data.Doctors
            //    .Where(d => d.UserId == this.User.GetId())
            //    .Select(d => d.Id)
            //    .FirstOrDefault();

            //    var doctor = this.data.Doctors.Find(doctorId);

            //    var petDoctor = new PetDoctor
            //    {
            //        DoctorId = doctorId,
            //        PetId = petData.Id
            //    };

            //    doctor.PetDoctors.Add(petDoctor);
            //    petData.PetDoctors.Add(petDoctor);
            //    data.PetDoctors.Add(petDoctor);
            //}

            data.SaveChanges();

            return RedirectToAction("Mine", "Pets");
        }

        private bool UserIsClient()
        {
            return this.data
                .Clients
                .Any(c => c.UserId == this.User.GetId());
        }

        private bool UserIsDoctor()
        {
            return this.data
                .Doctors
                .Any(d => d.UserId == this.User.GetId());
        }

        private ICollection<PetTypeServiceModel> GetPetTypes()
        {
            return data.PetTypes
                .Select(p => new PetTypeServiceModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }
    }
}
