using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
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
