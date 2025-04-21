using EventCorp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventCorpModels
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        public string SelectedRole { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
