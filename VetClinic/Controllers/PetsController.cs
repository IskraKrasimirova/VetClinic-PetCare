﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Extensions;
using VetClinic.Core.Models.Pets;

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

            return View(new AddPetFormModel
            {
                PetTypes = GetPetTypes()
            });
        }

        public IActionResult All()
        {
            var pets = this.data.Pets
                .Select(p => new PetListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth,
                    PetType = p.PetType.Name,
                    Breed = p.Breed,
                    Gender = p.Gender.ToString()
                })
                .ToList();

            return View(pets);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddPetFormModel pet)
        {
            var clientId = this.data.Clients
                .Where(c => c.UserId == this.User.GetId())
                .Select(c => c.Id)
                .FirstOrDefault();

            if (clientId == null)
            {
                return BadRequest();
            }

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
                Breed = pet.Breed,
                Gender = pet.Gender,
                Description = pet.Description,
                PetTypeId = pet.PetTypeId,
                ClientId = clientId 
            };

            data.Pets.Add(petData);
            data.SaveChanges();

            return RedirectToAction("All", "Pets");
        }

        private bool UserIsClient()
        {
            return this.data
                .Clients
                .Any(c => c.UserId == this.User.GetId());
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
