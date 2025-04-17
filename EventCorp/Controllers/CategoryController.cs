using EventCorp.Models;
using EventCorpModels.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventCorp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CE2DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(CE2DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Category.Include(c => c.Usuario).ToListAsync();
            return View(categorias);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Category
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category categoria)
        {
            if (ModelState.IsValid)
            {
                categoria.FechaRegistro = DateTime.Now;
                var user = await _userManager.GetUserAsync(User);
                categoria.UsuarioId = user?.Id;

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Category.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category categoria)
        {
            if (id != categoria.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Category.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                    categoria.UsuarioId = original?.UsuarioId; // preserva el usuario original
                    categoria.FechaRegistro = original?.FechaRegistro ?? DateTime.Now; // preserva la fecha original

                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Category.Any(e => e.Id == categoria.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Category
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Category.FindAsync(id);
            if (categoria != null)
            {
                _context.Category.Remove(categoria);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}