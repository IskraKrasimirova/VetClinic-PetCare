using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Pets
{
    public class AllPetsViewModel
    {
        public const int PetsPerPage = 3;

        [Display(Name = "Pet Type")]
        public string PetTypeName { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public int CurrentPage { get; init; } = 1;

        public int TotalPets { get; set; }

        public IEnumerable<string> PetTypes { get; set; }

        public IEnumerable<PetListingViewModel> Pets { get; set; }
    }
}
