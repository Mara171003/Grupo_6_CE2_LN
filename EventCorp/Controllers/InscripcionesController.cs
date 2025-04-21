
using EventCorp.Models;
using EventCorpModels;
using EventCorpModels.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventCorp.Controllers
{
    [Authorize]
    public class InscripcionesController : Controller
    {
        private readonly CE2DbContext _context;
        private readonly UserManager<User> _userManager;

        public InscripcionesController(CE2DbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Inscribirse(int eventoId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var evento = await _context.Eventos.FindAsync(eventoId);
            if (evento == null) return NotFound();

            // 1. Validar si el evento está lleno
            var inscripcionesActuales = await _context.Inscripciones
                .CountAsync(i => i.EventoId == eventoId);
            if (inscripcionesActuales >= evento.CupoMaximo)
            {
                TempData["Error"] = "El evento ya ha alcanzado el cupo máximo.";
                return RedirectToAction("Details", "Eventos", new { id = eventoId });
            }

            // 2. Validar si el usuario ya está inscrito
            var yaInscrito = await _context.Inscripciones
                .AnyAsync(i => i.EventoId == eventoId && i.UserId == user.Id);
            if (yaInscrito)
            {
                TempData["Error"] = "Ya estás inscrito en este evento.";
                return RedirectToAction("Details", "Eventos", new { id = eventoId });
            }

            // 3. Validar que no se solape con otro evento al que el usuario ya esté inscrito
            var eventosInscritos = await _context.Inscripciones
                .Include(i => i.Evento)
                .Where(i => i.UserId == user.Id)
                .Select(i => i.Evento)
                .ToListAsync();

            foreach (var e in eventosInscritos)
            {
                // Si es el mismo día
                if (e.Fecha == evento.Fecha)
                {
                    var horaInicio1 = e.Hora;
                    var horaFin1 = e.Hora.Add(TimeSpan.FromMinutes(e.Duracion));

                    var horaInicio2 = evento.Hora;
                    var horaFin2 = evento.Hora.Add(TimeSpan.FromMinutes(evento.Duracion));

                    // Validar solapamiento de horarios
                    bool seSolapan = horaInicio1 < horaFin2 && horaInicio2 < horaFin1;

                    if (seSolapan)
                    {
                        TempData["Error"] = $"Ya estás inscrito en otro evento que se superpone en horario.";
                        return RedirectToAction("Details", "Eventos", new { id = eventoId });
                    }
                }
            }

            // Si todo es válido, crear inscripción
            var inscripcion = new Inscripcion
            {
                UserId = user.Id,
                EventoId = eventoId,
                FechaInscripcion = DateTime.Now
            };

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Te has inscrito al evento exitosamente.";
            return RedirectToAction("Details", "Eventos", new { id = eventoId });
        }
    }
}
