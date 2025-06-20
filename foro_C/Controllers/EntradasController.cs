using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace foro_C.Controllers
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

        // =========================================
        // LISTAR (anónimo)
        // =========================================
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var entradas = await _context.Entradas
                                         .Where(e => e.Activa && !e.Privada)
                                         .Include(e => e.Miembro)
                                         .Include(e => e.Categoria)
                                         .Include(e => e.Preguntas)
                                         .ToListAsync();

            return View(entradas);
        }

        // =========================================
        // DETALLE (anónimo, sólo públicas)
        // =========================================
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entrada = await _context.Entradas
                                        .Include(e => e.Categoria)
                                        .Include(e => e.Miembro)
                                        .Include(e => e.Preguntas!)
                                            .ThenInclude(p => p.Respuestas)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (entrada == null) return NotFound();

            // Si es privada y el usuario no está habilitado → acceso denegado
            if (entrada.Privada && !UserCanAccessPrivateEntry(entrada))
                return RedirectToAction("AccesoDenegado", "Account");

            return View(entrada);
        }

        // =========================================
        // CREAR
        // =========================================
        [Authorize(Roles = "Miembro")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro")]
        public async Task<IActionResult> Create([Bind("Titulo,Texto,Privada,CategoriaId")] Entrada entrada)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
                return View(entrada);
            }

            entrada.Fecha = DateTime.UtcNow;
            entrada.Activa = true;
            entrada.MiembroId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            _context.Add(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =========================================
        // EDITAR
        // =========================================
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null) return NotFound();

            if (!EsPropietarioOAdmin(entrada))
                return Forbid(); // o RedirectToAction("AccesoDenegado","Account")

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
            return View(entrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Texto,Privada,CategoriaId")] Entrada entrada)
        {
            if (id != entrada.Id) return NotFound();

            var entradaOriginal = await _context.Entradas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (entradaOriginal == null) return NotFound();

            if (!EsPropietarioOAdmin(entradaOriginal))
                return Forbid();

            entrada.MiembroId = entradaOriginal.MiembroId; // evitar cambio forzado
            entrada.Fecha = entradaOriginal.Fecha;     // preservar fecha original

            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
                return View(entrada);
            }

            _context.Update(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entrada = await _context.Entradas
                                        .Include(e => e.Categoria)
                                        .Include(e => e.Miembro)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null) return NotFound();

            return View(entrada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Miembro")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada != null) _context.Entradas.Remove(entrada);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 
        private bool EntradaExists(int id) =>
            _context.Entradas.Any(e => e.Id == id);

        private bool EsPropietarioOAdmin(Entrada entrada)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return entrada.MiembroId == currentUserId || User.IsInRole("Administrador");
        }

        private bool UserCanAccessPrivateEntry(Entrada entrada)
        {
            if (!entrada.Privada) return true;
            if (User.IsInRole("Administrador")) return true;

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return _context.Habilitaciones.Any(h => h.EntradaId == entrada.Id && h.MiembroId == currentUserId);
        }
    }
}



