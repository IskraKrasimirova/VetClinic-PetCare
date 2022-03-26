using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Pets;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;

namespace VetClinic.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService petService;
        private readonly IPetTypeService petTypeService;

        public PetsController(IPetService petService, IPetTypeService petTypeService)
        {
            this.petService = petService;
            this.petTypeService = petTypeService;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.User.IsClient())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new PetFormModel
            {
                PetTypes = this.petTypeService.GetPetTypes()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(PetFormModel pet)
        {
            var clientId = petService.GetClientId(this.User.GetId());

            if (!petService.PetTypeExists(pet.PetTypeId))
            {
                this.ModelState.AddModelError(nameof(pet.PetTypeId), "Pet type does not exist.");
            }

            if (!ModelState.IsValid)
            {
                pet.PetTypes = this.petTypeService.GetPetTypes();

                return View(pet);
            }

            var petId = petService.AddPet(
                pet.Name,
                pet.DateOfBirth,
                pet.Breed,
                pet.Gender.ToString(),
                pet.Description,
                pet.PetTypeId,
                clientId);

            return RedirectToAction("Mine", "Pets");
            //return RedirectToAction("Details", "Pets");
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult All([FromQuery]AllPetsViewModel query)
        {
            var queryResult = petService.All(
                query.SearchTerm,
                query.PetTypeName,
                query.CurrentPage,
                AllPetsViewModel.PetsPerPage);

            var petPetTypes = petService.AllPetTypes();

            query.TotalPets = queryResult.TotalPets;
            query.Pets = queryResult.Pets;
            query.PetTypes = petPetTypes;

            return View(query);
        }

        [Authorize(Roles = ClientRoleName)]
        public IActionResult Mine()
        {
            var clientId = petService.GetClientId(this.User.GetId());

            if (clientId == null)
            {
                return BadRequest();
            }

            var myPets = petService.ByClient(clientId);

            return View(myPets);
        }

        
    }
}
