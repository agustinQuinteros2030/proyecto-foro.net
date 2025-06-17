using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CategoriasController : Controller
    {
        private readonly ForoContext _context;

        public CategoriasController(ForoContext context)
        {
            _context = context;
        }

        // GET: Categorias
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return View(categorias);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay error, volvemos a mostrar el formulario con los errores
            return View(categoria);
        }





        // GET: Categorias/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
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

        // GET: Categorias/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Titulo,Texto,Privada,CategoriaId")] Entrada formEntrada)
        {
            if (id != formEntrada.Id) return NotFound();

            // 1. Obtenemos la instancia ORIGINAL que está trackeada
            var entradaOriginal = await _context.Entradas
                                                .Include(e => e.Categoria)
                                                .FirstOrDefaultAsync(e => e.Id == id);

            if (entradaOriginal == null) return NotFound();

            // 2. Seguridad: sólo dueño o admin
            if (!EsPropietarioOAdmin(entradaOriginal)) return Forbid();

            // 3. Actualizamos propiedades UNA POR UNA (sin reemplazar la instancia)
            entradaOriginal.Titulo = formEntrada.Titulo;
            entradaOriginal.Texto = formEntrada.Texto;
            entradaOriginal.Privada = formEntrada.Privada;
            entradaOriginal.CategoriaId = formEntrada.CategoriaId;


            // 4. Guardamos – NO hace falta Attach/Update porque entradaOriginal ya está trackeada
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        [Authorize]
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

        private bool EsPropietarioOAdmin(Entrada entrada)
        {


            var usuarioLogueado = User.Identity.Name;

            // Verificamos si el usuario tiene rol administrador
            if (User.IsInRole("Administrador"))
                return true;

            // Verificamos si el usuario es el dueño (propietario) de la entrada

            if (entrada.Miembro != null && entrada.Miembro.UserName == usuarioLogueado)
                return true;

            return false;

        }
    }
}
