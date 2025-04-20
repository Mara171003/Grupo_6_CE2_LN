using System.ComponentModel.DataAnnotations;

namespace EventCorp.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }
    }
}
