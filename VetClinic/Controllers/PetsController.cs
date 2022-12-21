using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            //Валидационният атрибут не работи, защото трябва да се зададе const MaxDate!?
            //if (DateTime.Compare(DateTime.UtcNow.Date, pet.DateOfBirth.Date) < 0)
            //{
            //    this.ModelState.AddModelError(String.Empty, "The Date of Birth must be before the current date.");
            //}

            if (pet.DateOfBirth > DateTime.UtcNow)
            {
                this.ModelState.AddModelError(String.Empty, "The Date of Birth must be before the current date.");
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

            this.TempData[GlobalMessageKey] = "Successfully added a pet!";

            return RedirectToAction("Details", new { id = petId });
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult All([FromQuery] AllPetsViewModel query)
        {
            var queryResult = petService.All(
                query.PetTypeName,
                query.SearchTerm,
                query.CurrentPage,
                AllPetsViewModel.PetsPerPage);

            var petPetTypes = petService.AllPetTypes();

            query.PetTypes = petPetTypes;
            query.TotalPets = queryResult.TotalPets;
            query.Pets = queryResult.Pets;
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

        [Authorize(Roles = $"{ClientRoleName}, {DoctorRoleName}")]
        public IActionResult Edit(string id)
        {
            var pet = petService.Details(id);

            if (pet == null)
            {
                return BadRequest();
            }

            if (!this.User.IsClient() && !this.User.IsDoctor())
            {
                return RedirectToAction("Index", "Home");
            }

            var clientId = clientService.GetClientId(this.User.GetId());

            if (pet.ClientId != clientId && !this.User.IsDoctor())
            {
                return Unauthorized();
            }

            return View(new PetFormModel
            {
                Name = pet.Name,
                DateOfBirth = DateTime.ParseExact(pet.DateOfBirth, DateFormat, CultureInfo.InvariantCulture),
                Breed = pet.Breed,
                Gender = (Gender)Enum.Parse(typeof(Gender), pet.Gender),
                Description = pet.Description,
                PetTypeId = pet.PetTypeId,
                PetTypes = this.petTypeService.GetPetTypes()
            });
        }

        [HttpPost]
        [Authorize(Roles = $"{ClientRoleName}, {DoctorRoleName}")]
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

            if (pet.DateOfBirth > DateTime.UtcNow)
            {
                this.ModelState.AddModelError(String.Empty, "The Date of Birth must be before the current date.");
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

            this.TempData[GlobalMessageKey] = "Successfully edited a pet!";
            //return RedirectToAction("Mine", "Pets"); 
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = $"{ClientRoleName}, {DoctorRoleName}")]
        public IActionResult Details(string id)
        {
            var pet = this.petService.Details(id);

            if (pet == null)
            {
                return BadRequest();
            }

            if (!this.User.IsClient() && !this.User.IsDoctor())
            {
                return RedirectToAction("Index", "Home");
            }

            if (this.User.IsClient())
            {
                var clientId = clientService.GetClientId(this.User.GetId());

                if (pet.ClientId != clientId)
                {
                    return Unauthorized();
                }
            }

            return View(pet);
        }


        [Authorize(Roles = ClientRoleName)]
        public IActionResult Delete(string id)
        {
            var pet = this.petService.GetPetForDelete(id);

            if (pet == null)
            {
                return BadRequest();
            }

            if (!this.User.IsClient())
            {
                return RedirectToAction("Index", "Home");
            }

            var clientId = clientService.GetClientId(this.User.GetId());

            if (!this.petService.IsByOwner(id, clientId))
            {
                return Unauthorized();
            }

            return View(pet);
        }

        [HttpPost]
        [Authorize(Roles = ClientRoleName)]
        public IActionResult DeletePet(string id)
        {
            if (!this.User.IsClient())
            {
                return RedirectToAction("Index", "Home");
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

            this.TempData[GlobalMessageKey] = "Successfully deleted a pet!";

            return RedirectToAction("Mine", "Pets");
        }
    }
}
