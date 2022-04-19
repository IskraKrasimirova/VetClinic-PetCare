using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VetClinic.Core.Contracts;
using VetClinic.Core.Models.Pets;
using VetClinic.Data.Enums;
using VetClinic.Extensions;
using static VetClinic.Common.GlobalConstants;
using static VetClinic.Common.GlobalConstants.FormattingConstants;


namespace VetClinic.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService petService;
        private readonly IPetTypeService petTypeService;
        private readonly IClientService clientService;

        public PetsController(IPetService petService, IPetTypeService petTypeService, IClientService clientService)
        {
            this.petService = petService;
            this.petTypeService = petTypeService;
            this.clientService = clientService;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.User.IsClient())
            {
                return RedirectToAction("Register", "Home");
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
            var clientId = clientService.GetClientId(this.User.GetId());

            if (clientId == null)
            {
                return RedirectToAction("Register", "Home");
            }

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

            return RedirectToAction("Details", new { id = petId });
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult All([FromQuery] AllPetsViewModel query)
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
            query.SearchTerm = queryResult.SearchTerm;

            return View(query);
        }

        [Authorize(Roles = ClientRoleName)]
        public IActionResult Mine()
        {
            var clientId = clientService.GetClientId(this.User.GetId());

            if (clientId == null)
            {
                return BadRequest();
            }

            var myPets = petService.ByClient(clientId);

            return View(myPets);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!this.User.IsClient() && !this.User.IsDoctor())
            {
                return RedirectToAction("Index", "Home");
            }

            var pet = petService.Details(id);

            var clientId = clientService.GetClientId(this.User.GetId());

            if (pet.ClientId != clientId && !this.User.IsDoctor())
            {
                return Unauthorized();
            }

            return View(new PetFormModel
            {
                Name = pet.Name,
                DateOfBirth = DateTime.ParseExact(pet.DateOfBirth, NormalDateFormat, CultureInfo.InvariantCulture),
                Breed = pet.Breed,
                Gender = (Gender)Enum.Parse(typeof(Gender), pet.Gender),
                Description = pet.Description,
                PetTypeId = pet.PetTypeId,
                PetTypes = this.petTypeService.GetPetTypes()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string id, PetFormModel pet)
        {
            if (!this.User.IsClient() && !this.User.IsDoctor())
            {
                return RedirectToAction("Index", "Home");
            }

            var clientId = clientService.GetClientId(this.User.GetId());

            if (!petService.PetTypeExists(pet.PetTypeId))
            {
                this.ModelState.AddModelError(nameof(pet.PetTypeId), "Pet type does not exist.");
            }

            if (!ModelState.IsValid)
            {
                pet.PetTypes = this.petTypeService.GetPetTypes();

                return View(pet);
            }

            if (!this.petService.IsByOwner(id, clientId) && !this.User.IsDoctor())
            {
                return Unauthorized();
            }

            var isEdited = petService.Edit(
                id,
                pet.Name,
                pet.DateOfBirth,
                pet.Breed,
                pet.Gender.ToString(),
                pet.Description,
                pet.PetTypeId);

            if (!isEdited)
            {
                return BadRequest();
            }

            //return RedirectToAction("Mine", "Pets"); 
            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var pet = this.petService.Details(id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        [Authorize(Roles = ClientRoleName)]
        public IActionResult Delete(string id)
        {
            if (!this.User.IsClient())
            {
                return BadRequest();
            }

            var clientId = clientService.GetClientId(this.User.GetId());

            if (!this.petService.IsByOwner(id, clientId))
            {
                return Unauthorized();
            }

            var isDeleted = this.petService.Delete(id, clientId);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction("Mine", "Pets");
        }
    }
}
