using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VetClinic.Data.ModelConstants.User;

namespace VetClinic.Data.Models
{
    public class User : IdentityUser
    {
        //public User()
        //{
        //    //this.Id = Guid.NewGuid().ToString();
        //    this.Roles = new HashSet<IdentityUserRole<string>>();
        //    this.Claims = new HashSet<IdentityUserClaim<string>>();
        //}

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        //public Client Client { get; set; }
        //public Doctor Doctor { get; set; }

        //public ICollection<IdentityUserRole<string>> Roles { get; set; }

        //public ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }
}
