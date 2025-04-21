using EventCorp.Models;
using EventCorpModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventCorp.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        public Category Categoria { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required]
        public int Duracion { get; set; }  // Duración en minutos

        public string Ubicacion { get; set; }

        public int CupoMaximo { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public string UsuarioId { get; set; }

        public User Usuario { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }  // Relación con Inscripción
    }
}
