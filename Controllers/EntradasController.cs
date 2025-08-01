using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foro2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Foro2._0.Controllers
{
    public class EntradasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public EntradasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Entradas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Entradas.Include(e => e.Miembro).Include(e => e.Categoria);
            return View(await foroContext.ToListAsync());
        }

        // GET: Entradas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var entrada = await _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                        .ThenInclude(r => r.Reacciones)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entrada == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Check si el usuario es el autor de la entrada
            ViewBag.EsAutor = user != null && entrada.AutorId == user.Id;

            // También, si el usuario está habilitado para ver (o es autor)
            ViewBag.EstaHabilitado = user != null &&
                (entrada.AutorId == user.Id || await _context.Habilitaciones.AnyAsync(h => h.EntradaId == id && h.MiembroId == user.Id && h.Habilitado));

            // Solo si es autor, traer solicitudes pendientes
            if ((bool)ViewBag.EsAutor)
            {
                ViewBag.SolicitudesPendientes = await _context.Habilitaciones
                    .Include(h => h.Miembro)
                    .Where(h => h.EntradaId == id && !h.Habilitado)
                    .ToListAsync();
            }

            return View(entrada);
        }




        [Authorize(Roles = "MIEMBRO")]
        public IActionResult Create(string returnUrl = null)
        {
            ViewBag.Categorias = new SelectList(_context.Categorias.OrderBy(c => c.Nombre).ToList(), "Id", "Nombre");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Create(Entrada entrada, string TextoPregunta, int? CategoriaId, string NuevaCategoria, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
                return View(entrada);
            }

            // Aseguramos que el autor sea el usuario actual (ignorar cualquier valor recibido)
            var user = await _userManager.GetUserAsync(User);
            entrada.AutorId = user.Id;

            // Seteamos fecha
            entrada.FechaCreacion = DateTime.Now;

            // Categoria: si viene nueva, crearla
            if (!string.IsNullOrWhiteSpace(NuevaCategoria))
            {
                var catExistente = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == NuevaCategoria);
                if (catExistente == null)
                {
                    var nuevaCat = new Categoria { Nombre = NuevaCategoria };
                    _context.Categorias.Add(nuevaCat);
                    await _context.SaveChangesAsync(); // Guardar para que tenga Id
                    entrada.CategoriaId = nuevaCat.Id;
                }
                else
                {
                    entrada.CategoriaId = catExistente.Id;
                }
            }
            else if (CategoriaId.HasValue)
            {
                entrada.CategoriaId = CategoriaId.Value;
            }
            else
            {
                ModelState.AddModelError("", "Debe seleccionar o crear una categoría.");
                ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
                return View(entrada);
            }

            // Crear la pregunta inicial
            var pregunta = new Pregunta
            {
                Texto = TextoPregunta,
                Fecha = DateTime.Now,
                Activa = true,
                MiembroId = user.Id, // El mismo autor
                Entrada = entrada
            };

            entrada.Preguntas.Add(pregunta);

            _context.Entradas.Add(entrada);
            await _context.SaveChangesAsync();

            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }



        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entrada = await _context.Entradas
                .Include(e => e.Miembro) // Necesario para comparar el autor
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entrada == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (entrada.AutorId != user.Id)
                return Forbid();

            ViewData["AutorId"] = new SelectList(_context.Miembros, "Id", "Apellido", entrada.AutorId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
            return View(entrada);
        }


        // POST: Entradas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Texto,EsPrivada,FechaCreacion,AutorId,CategoriaId")] Entrada entrada)
        {
            if (id != entrada.Id) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var original = await _context.Entradas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            if (original == null || original.AutorId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AutorId"] = new SelectList(_context.Miembros, "Id", "Apellido", entrada.AutorId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
            return View(entrada);
        }


        // GET: Entradas/Delete/5
        [Authorize(Roles = "MIEMBRO,ADMINISTRADOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entrada = await _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entrada == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Si es miembro, solo puede borrar su propia entrada
            if (User.IsInRole("MIEMBRO") && entrada.AutorId != user.Id)
                return Forbid();

            return View(entrada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO,ADMINISTRADOR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            // Si es miembro, solo puede borrar su propia entrada
            if (User.IsInRole("MIEMBRO") && entrada.AutorId != user.Id)
                return Forbid();

            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool EntradaExists(int id)
        {
            return _context.Entradas.Any(e => e.Id == id);
        }
    }
}
