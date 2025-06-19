using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    [Authorize(Roles = "Administrador")]          // ⚠️ solo admin para todas las acciones salvo las que marquemos con [AllowAnonymous]
    public class CategoriasController : Controller
    {
        private readonly ForoContext _context;

        public CategoriasController(ForoContext context)
        {
            _context = context;
        }

        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
                                           .OrderBy(c => c.Nombre)
                                           .ToListAsync();
            return View(categorias);
        }

      
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias
                                          .Include(c => c.Entradas)   // si querés mostrar cuántas entradas tiene
                                          .FirstOrDefaultAsync(c => c.Id == id);

            return categoria is null ? NotFound() : View(categoria);
        }

    
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre")] Categoria categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();
            if (!ModelState.IsValid) return View(categoria);

            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias
                                          .FirstOrDefaultAsync(c => c.Id == id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria is not null) _context.Categorias.Remove(categoria);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        private bool CategoriaExists(int id) =>
            _context.Categorias.Any(e => e.Id == id);
    }
}

