using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Pets
{
    public class PetDeleteServiceModel : PetListingViewModel
    {
        public string Description { get; set; }

        public int PetTypeId { get; set; }

        public string ClientId { get; set; }
    }
}
