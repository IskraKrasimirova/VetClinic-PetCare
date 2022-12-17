using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using NUnit.Framework;
using VetClinic.Controllers;
using VetClinic.Core.Models.Pets;
using VetClinic.Core.Models.PetTypes;
using VetClinic.Core.Services;
using VetClinic.Data;
using VetClinic.Data.Models;
using VetClinic.Test.Mocks;
using static VetClinic.Common.GlobalConstants;
using PetService = VetClinic.Core.Services.PetService;

namespace VetClinic.Test.ControllerTests
{
    public class PetsControllerTests
    {
        private VetClinicDbContext dbContext;
        private PetService service;
        private PetTypeService petTypeService;
        private ClientService clientService;
        private PetsController controller;

        [SetUp]
        public void Setup()
        {
            dbContext = DatabaseMock.Instance;
            petTypeService = new PetTypeService(dbContext);
            clientService = new ClientService(dbContext);
            service = new PetService(dbContext);
            controller = new PetsController(service, petTypeService, clientService);
        }

        [TearDown]
        public void Dispose()
        {
            dbContext.Dispose();
        }

        [Test]
        public void AddShouldReturnViewWithModelWhenUserIsClient()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };
            var result = controller.Add();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetFormModel>();
        }

        [Test]
        public void AddShouldRedirectToActionWhenUserIsNotClient()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Add();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void AddPostShouldRedirectToActionWhenClientNotExist()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };
            var petFormModel = new PetFormModel
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>
                {
                    new PetTypeServiceModel
                    {
                        Id = 1,
                        Name = "Cat"
                    }
                }
            };
            var result = controller.Add(petFormModel);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void AddPostShouldReturnViewWithErrorMessageWhenModelStateIsNotValid()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };
            var petFormModel = new PetFormModel
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>()
            };
            var result = controller.Add(petFormModel);
            Assert.That(controller.ViewData.ModelState.ErrorCount, Is.EqualTo(2));
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetFormModel>();
        }

        [Test]
        public void AddPostShouldRedirectToActionWhenModelStateIsValid()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            dbContext.Clients.Add(client);

            var petTypes = new List<PetType>
            {
                new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                new PetType
                {
                    Id = 2,
                    Name = "Dog"
                }
            };

            dbContext.PetTypes.AddRange(petTypes);
            dbContext.SaveChanges();

            controller.TempData = TempDataMock.Instance;
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var petFormModel = new PetFormModel
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>()
                {
                    new PetTypeServiceModel
                    {
                        Id = 1,
                        Name = "Cat"
                    },
                    new PetTypeServiceModel
                    {
                        Id = 2,
                        Name = "Dog"
                    }
                }
            };
            var result = controller.Add(petFormModel);
            var petId = service.AddPet(petFormModel.Name, petFormModel.DateOfBirth, petFormModel.Breed, petFormModel.Gender.ToString(), null, petFormModel.PetTypeId, client.Id);
            Assert.That(petId, Is.Not.Null);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void AllShouldReturnViewWithModelWhenUserIsDoctor()
        {
            var user = new User
            {
                Id = "TestUserId",
                Email = "testDoctor@vetclinic.com",
                UserName = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName"
            };

            dbContext.Users.Add(user);

            var doctor = new Doctor
            {
                Id = "TestDoctorId",
                FullName = "TestDoctor FullName",
                DepartmentId = 1,
                Department = new Department
                {
                    Id = 1,
                    Name = "TestDepartment",
                    Image = "TestDepartmentImg.png"
                },
                UserId = user.Id,
                Description = "some description",
                Email = "testDoctor@petcare.com",
                PhoneNumber = "0888555666",
                ProfileImage = "testProfileImg.png"
            };
            dbContext.Doctors.Add(doctor);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, DoctorRoleName)
            };

            var petTypes = new List<string>();
            var pets = new List<PetListingViewModel>();
            var model = new AllPetsViewModel
            {
                CurrentPage = 1,
                PetTypes = petTypes,
                Pets = pets,
                PetTypeName = "Cat",
                TotalPets = 0
            };

            var result = controller.All(model);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<AllPetsViewModel>();
        }

        [Test]
        public void MineShouldReturnViewWithModelWhenUserIsClient()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                ClientId = client.Id
            };

            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.Mine();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<List<PetListingViewModel>>();
        }

        [Test]
        public void MineShouldReturnBadRequestWhenUserIsNotClient()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Mine();
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<BadRequestResult>();
        }

        [Test]
        public void EditShouldReturnViewWithModelWhenUserIsOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = client.Id,
                Client = client
            };

            client.Pets.Add(pet);
            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.Edit(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetFormModel>();
        }

        [Test]
        public void EditShouldReturnViewWithModelWhenUserIsDoctort()
        {
            var user = new User
            {
                Id = "TestUserId",
                Email = "testDoctor@vetclinic.com",
                UserName = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName"
            };

            dbContext.Users.Add(user);

            var doctor = new Doctor
            {
                Id = "TestDoctorId",
                FullName = user.FullName,
                DepartmentId = 1,
                Department = new Department
                {
                    Id = 1,
                    Name = "TestDepartment",
                    Image = "TestDepartmentImg.png"
                },
                UserId = user.Id,
                Description = "some description",
                Email = user.Email,
                PhoneNumber = "0888555666",
                ProfileImage = "testProfileImg.png"
            };
            dbContext.Doctors.Add(doctor);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, DoctorRoleName)
            };

            var pet = new Pet
            {
                Id = "TestPetId",
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId",
                Client = new Client
                {
                    Id = "testClientId",
                    UserId = "testUserId",
                    FullName = "TestName"
                }
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            var result = controller.Edit(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetFormModel>();
        }

        [Test]
        public void EditShouldRedirectToActionWhenUserIsNotClientOrDoctort()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Id = "TestPetId",
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId",
                Client = new Client
                {
                    Id = "testClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Edit(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void EditShouldReturnUnauthorizedWhenClientIsNotOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance( user.Id, ClientRoleName)
            };

            var result = controller.Edit(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void EditShouldReturnBadRequestWhenPetNotExist()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Edit("NotExistingPet");
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<BadRequestResult>();
        }

        [Test]
        public void EditPostShouldRedirectToActionWhenValid()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var petTypes = new List<PetType>
            {
                new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                new PetType
                {
                    Id = 2,
                    Name = "Dog"
                }
            };

            dbContext.PetTypes.AddRange(petTypes);

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                ClientId = client.Id,
                Client = client
            };

            client.Pets.Add(pet);
            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.TempData = TempDataMock.Instance;
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var petFormModel = new PetFormModel
            {
                Name = "NewTestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>()
                {
                    new PetTypeServiceModel
                    {
                        Id = 1,
                        Name = "Cat"
                    },
                    new PetTypeServiceModel
                    {
                        Id = 2,
                        Name = "Dog"
                    }
                }
            };

            var result = controller.Edit(pet.Id, petFormModel);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void EditPostShouldReturnViewWithErrorMessageWhenModelStateIsNotValid()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                ClientId = client.Id,
                Client = client
            };

            client.Pets.Add(pet);
            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var petFormModel = new PetFormModel
            {
                Name = "NewTestPet",
                DateOfBirth = DateTime.Now.AddYears(2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>()
            };

            var result = controller.Edit(pet.Id, petFormModel);
            Assert.That(controller.ViewData.ModelState.ErrorCount, Is.EqualTo(2));
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetFormModel>();
        }

        [Test]
        public void EditPostShouldRedirectToActionWhenUserIsNotClientOrDoctor()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId"
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var petFormModel = new PetFormModel
            {
                Name = "NewTestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>()
                {
                    new PetTypeServiceModel
                    {
                        Id = 1,
                        Name = "Cat"
                    },
                    new PetTypeServiceModel
                    {
                        Id = 2,
                        Name = "Dog"
                    }
                }
            };

            var result = controller.Edit(pet.Id, petFormModel);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void EditPostShouldReturnUnauthorizedWhenClientIsNotOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var petFormModel = new PetFormModel
            {
                Name = "NewTestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                PetTypes = new List<PetTypeServiceModel>()
                {
                    new PetTypeServiceModel
                    {
                        Id = 1,
                        Name = "Cat"
                    },
                    new PetTypeServiceModel
                    {
                        Id = 2,
                        Name = "Dog"
                    }
                }
            };

            var result = controller.Edit(pet.Id, petFormModel);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void DetailsShouldReturnViewWhenUserIsOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };
            
            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId",
                Client = client
            };

            client.Pets.Add(pet);
            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName),
            };

            var result = controller.Details(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetDetailsServiceModel>();
        }

        [Test]
        public void DetailsShouldReturnViewWithModelWhenUserIsDoctort()
        {
            var user = new User
            {
                Id = "TestUserId",
                Email = "testDoctor@vetclinic.com",
                UserName = "testDoctor@vetclinic.com",
                FullName = "TestDoctorFullName"
            };

            dbContext.Users.Add(user);

            var doctor = new Doctor
            {
                Id = "TestDoctorId",
                FullName = user.FullName,
                DepartmentId = 1,
                Department = new Department
                {
                    Id = 1,
                    Name = "TestDepartment",
                    Image = "TestDepartmentImg.png"
                },
                UserId = user.Id,
                Description = "some description",
                Email = user.Email,
                PhoneNumber = "0888555666",
                ProfileImage = "testProfileImg.png"
            };
            dbContext.Doctors.Add(doctor);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, DoctorRoleName)
            };

            var pet = new Pet
            {
                Id = "TestPetId",
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId",
                Client = new Client
                {
                    Id = "testClientId",
                    UserId = "testUserId",
                    FullName = "TestName"
                }
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            var result = controller.Details(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetDetailsServiceModel>();
        }

        [Test]
        public void DetailsShouldReturnBadRequestWhenPetNotExist()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Id = "TestPetId",
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId"
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Details("NotExistingPet");
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<BadRequestResult>();
        }

        [Test]
        public void DetailsShouldRedirectToActionWhenUserIsNotClientOrDoctor()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Details(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void DetailsShouldReturnUnauthorizedWhenClientIsNotOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.Details(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void DeleteShouldReturnBadRequestWhenPetNotExist()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Id = "TestPetId",
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId"
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Delete("NotExistingPet");
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<BadRequestResult>();
        }

        [Test]
        public void DeleteShouldRedirectToActionWhenUserIsNotClient()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.Delete(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void DeleteShouldReturnUnauthorizedWhenClientIsNotOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.Delete(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void DeleteShouldReturnViewWhenUserIsOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "testClientId",
                Client = client
            };

            client.Pets.Add(pet);
            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.Delete(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ViewResult>()
                .Which
                .Model
                .Should()
                .BeOfType<PetDeleteServiceModel>();
        }

        [Test]
        public void DeletePetShouldRedirectToActionWhenUserIsNotClient()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, "")
            };

            var result = controller.DeletePet(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }

        [Test]
        public void DeletePetShouldReturnUnauthorizedWhenClientIsNotOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = "NewTestClientId",
                Client = new Client
                {
                    Id = "NewTestClientId",
                    UserId = "NewTestUserId",
                    FullName = "NewTestName"
                }
            };

            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.DeletePet(pet.Id);
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void DeletePetShouldReturnRedirectToActionWhenUserIsOwner()
        {
            var user = new User
            {
                Id = "testUserId",
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "0888777111",
                FullName = "TestName"
            };

            dbContext.Users.Add(user);

            var client = new Client
            {
                Id = "testClientId",
                UserId = user.Id,
                FullName = user.FullName,
            };

            var pet = new Pet
            {
                Name = "TestPet",
                DateOfBirth = DateTime.Now.AddYears(-2),
                Breed = "Persian",
                PetTypeId = 1,
                PetType = new PetType
                {
                    Id = 1,
                    Name = "Cat"
                },
                Gender = Data.Enums.Gender.Male,
                ClientId = client.Id,
                Client = client
            };

            client.Pets.Add(pet);
            dbContext.Clients.Add(client);
            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            controller.TempData = TempDataMock.Instance;
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id, ClientRoleName)
            };

            var result = controller.DeletePet(pet.Id);
            var expectedPetsCount = dbContext.Pets.Count();
            Assert.That(expectedPetsCount, Is.EqualTo(0));
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<RedirectToActionResult>();
        }
    }
}
