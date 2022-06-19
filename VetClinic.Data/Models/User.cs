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
        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }
    }
}
