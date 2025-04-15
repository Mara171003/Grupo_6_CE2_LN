using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventCorpModels
{
    public class User : IdentityUser
    {
        [Key] 
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        public string SelectedRole { get; set; }
    }
}
