using Microsoft.AspNetCore.Mvc;
using EventCorp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventCorp.Controllers
{
    public class AsistenciaController : Controller
    {
        // Simulación de datos
        private List<Inscripcion> ObtenerInscripciones()
        {
            return new List<Inscripcion>
            {
                new Inscripcion { Id = 1, EventoId = 1, UserId = "user1", Asistio = false },
                new Inscripcion { Id = 2, EventoId = 1, UserId = "user2", Asistio = true },
                new Inscripcion { Id = 3, EventoId = 2, UserId = "user3", Asistio = false },
                new Inscripcion { Id = 4, EventoId = 3, UserId = "user4", Asistio = true },
            };
        }

        public IActionResult Index(int eventoId)
        {
            // Obtener solo las inscripciones de un evento específico
            var inscripciones = ObtenerInscripciones().Where(i => i.EventoId == eventoId).ToList();

            ViewBag.EventoId = eventoId;
            return View(inscripciones);
        }

        [HttpPost]
        public IActionResult MarcarAsistencia(int id)
        {
            // Aquí marcaríamos la asistencia real
            // En este ejemplo solo redirigimos (simulado)
            TempData["Mensaje"] = $"Asistencia marcada para inscripción #{id}";
            return RedirectToAction("Index", new { eventoId = 1 }); // puedes ajustar el evento
        }
    }
}
