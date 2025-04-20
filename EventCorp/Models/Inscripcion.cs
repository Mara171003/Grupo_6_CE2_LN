using EventCorpModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventCorp.Models
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User Usuario { get; set; }

        [Required]
        public int EventoId { get; set; }

        // Esta clase la creamos como temporal
        [ForeignKey("EventoId")]
        public Evento Evento { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public bool Asistio { get; set; } = false;
    }
}
