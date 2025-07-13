using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foro2._0.Models;
using Microsoft.AspNetCore.Authorization;

namespace Foro2._0.Controllers
{
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
      .Include(c => c.Entradas) // <-- esto es clave
      .OrderBy(c => c.Nombre)
      .ToListAsync();

            return View(categorias);
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .Include(c => c.Entradas)
                    .ThenInclude(e => e.Miembro)  // Incluimos el autor de la entrada
                                                  //.Include(c => c.Entradas)
                                                  //    .ThenInclude(e => e.Categoria)  // Esta línea la comentamos o eliminamos para evitar error
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }


        [Authorize(Roles = "ADMINISTRADOR,MIEMBRO")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR,MIEMBRO")]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Categoria categoria)
        {
            // Normalizar el nombre para comparar (sin espacios y en minúsculas)
            var nombreNormalizado = categoria.Nombre.Trim().ToLower();

            var existeCategoria = await _context.Categorias
                .AnyAsync(c => c.Nombre.Trim().ToLower() == nombreNormalizado);

            if (existeCategoria)
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }




        // GET: Categorias/Delete/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
