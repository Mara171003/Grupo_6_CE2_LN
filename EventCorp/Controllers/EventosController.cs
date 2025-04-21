using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventCorp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventCorpModels.Data;
using EventCorpModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EventCorp.Controllers
{
    public class EventosController : Controller
    {
        private readonly CE2DbContext _context;
        private readonly UserManager<User> _userManager;
        public EventosController(CE2DbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventos = _context.Eventos.Include(e => e.Categoria);
            return View(await eventos.ToListAsync());
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null) return NotFound();

            return View(evento);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            ViewData["Id_Categoria"] = new SelectList(_context.Category, "Id", "Nombre");
            return View();
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        {
            // Validaciones personalizadas
            if (evento.Fecha < DateTime.Today)
                ModelState.AddModelError("Fecha", "La fecha del evento no puede ser en el pasado.");

            if (evento.Duracion <= 0)
                ModelState.AddModelError("Duracion", "La duración debe ser mayor a 0.");

            if (evento.CupoMaximo <= 0)
                ModelState.AddModelError("CupoMaximoAsistencia", "El cupo máximo debe ser mayor a 0.");

            if (ModelState.IsValid)
            {
                evento.FechaRegistro = DateTime.Now;
                evento.UsuarioId = User.Identity?.Name ?? "Sistema";

                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Id_Categoria"] = new SelectList(_context.Category, "Id", "Nombre", evento.CategoriaId);
            return View(evento);
        }



    // GET: Eventos/Edit/5
    public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            ViewData["Id_Categoria"] = new SelectList(_context.Category, "Id", "Nombre", evento.CategoriaId);
            return View(evento);
        }

        // POST: Eventos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id)
                return NotFound();

            // Validaciones personalizadas
            if (evento.Fecha < DateTime.Today)
                ModelState.AddModelError("Fecha", "La fecha del evento no puede ser en el pasado.");

            if (evento.Duracion <= 0)
                ModelState.AddModelError("Duracion", "La duración debe ser mayor a 0.");

            if (evento.CupoMaximo <= 0)
                ModelState.AddModelError("CupoMaximoAsistencia", "El cupo máximo debe ser mayor a 0.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Eventos.Any(e => e.Id == evento.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Id_Categoria"] = new SelectList(_context.Category, "Id", "Nombre", evento.CategoriaId);
            return View(evento);
        }


        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evento == null) return NotFound();

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UsuariosPorEvento()
        {
            var eventos = await _context.Eventos
                .Include(e => e.Inscripciones)
                .ThenInclude(i => i.Usuario)  // Incluir los usuarios inscritos
                .ToListAsync();

            return View(eventos);
        }

        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> MisEventos()
        {
            var user = await _userManager.GetUserAsync(User);  // Obtener el usuario actual
            var eventos = await _context.Eventos
                .Where(e => e.UsuarioId == user.Id)  // Filtrar eventos creados por el organizador
                .Include(e => e.Inscripciones)
                .ThenInclude(i => i.Usuario)
                .ToListAsync();

            return View(eventos);
        }

    }
}
