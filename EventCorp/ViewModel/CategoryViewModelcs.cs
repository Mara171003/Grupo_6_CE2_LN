using System.ComponentModel.DataAnnotations;

namespace EventCorp.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaRegistro { get; set; }

        public string? UsuarioNombre { get; set; } 
    }
}