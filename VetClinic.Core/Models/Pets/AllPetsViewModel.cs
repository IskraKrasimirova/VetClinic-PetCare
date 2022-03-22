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
        [Display(Name = "Pet Type")]
        public string PetTypeName { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public IEnumerable<string> PetTypes { get; set; }

        public IEnumerable<PetListingViewModel> Pets { get; set; }
    }
}
