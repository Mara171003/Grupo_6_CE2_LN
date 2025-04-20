using Microsoft.AspNetCore.Mvc;
using EventCorp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventCorp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Simulamos algunos eventos
            var eventos = new List<Evento>
            {
                new Evento { Id = 1, Titulo = "Conferencia de Tecnología" },
                new Evento { Id = 2, Titulo = "Workshop de Marketing" },
                new Evento { Id = 3, Titulo = "Networking Empresarial" },
                new Evento { Id = 4, Titulo = "Taller de Finanzas" },
                new Evento { Id = 5, Titulo = "Evento de Lanzamiento" }
            };

            // Simulamos inscripciones (con fechas actuales y asistencias)
            var inscripciones = new List<Inscripcion>
            {
                new Inscripcion { EventoId = 1, FechaInscripcion = DateTime.Now.AddDays(-2), Asistio = true },
                new Inscripcion { EventoId = 1, FechaInscripcion = DateTime.Now.AddDays(-1), Asistio = true },
                new Inscripcion { EventoId = 2, FechaInscripcion = DateTime.Now, Asistio = false },
                new Inscripcion { EventoId = 3, FechaInscripcion = DateTime.Now, Asistio = true },
                new Inscripcion { EventoId = 3, FechaInscripcion = DateTime.Now, Asistio = true },
                new Inscripcion { EventoId = 4, FechaInscripcion = DateTime.Now, Asistio = true },
                new Inscripcion { EventoId = 5, FechaInscripcion = DateTime.Now, Asistio = false },
                new Inscripcion { EventoId = 5, FechaInscripcion = DateTime.Now, Asistio = true },
            };

            // Total eventos
            var totalEventos = eventos.Count;

            // Simulamos usuarios activos
            var totalUsuarios = 12; // simulado, puedes conectar esto al DbContext después

            // Asistentes registrados este mes
            var mesActual = DateTime.Now.Month;
            var asistentesMes = inscripciones
                .Where(i => i.FechaInscripcion.Month == mesActual)
                .Count();

            // Top 5 eventos por inscripciones
            var topEventos = inscripciones
                .GroupBy(i => i.EventoId)
                .Select(g => new
                {
                    EventoId = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(g => g.Total)
                .Take(5)
                .Join(eventos, g => g.EventoId, e => e.Id, (g, e) => new
                {
                    e.Titulo,
                    g.Total
                })
                .ToList();

            // Enviar todo al ViewBag (temporal y rápido)
            ViewBag.TotalEventos = totalEventos;
            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.AsistentesMes = asistentesMes;
            ViewBag.TopEventos = topEventos;

            return View();
        }
    }
}
