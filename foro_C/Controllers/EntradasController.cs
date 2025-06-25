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
using System.Collections.Generic;

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

   
        [AllowAnonymous]

        public async Task<IActionResult> Details(int id)
        {
            var entrada = await _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                        .ThenInclude(r => r.Miembro)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entrada == null)
                return NotFound();

            return View(entrada);
        }




        // =========================================
        // CREAR
        // =========================================
        [Authorize(Roles = "Miembro,Administrador")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        
        public async Task<IActionResult> Create([Bind("Titulo,Texto,Privada,CategoriaId")] Entrada entrada)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
                return View(entrada);
            }

            entrada.Fecha = DateTime.UtcNow;
            entrada.MiembroId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
           

            _context.Add(entrada);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); // ✅ Te lleva al inicio
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
            entrada.Fecha = entradaOriginal.Fecha;         // preservar fecha original

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


        [HttpGet]
        public async Task<IActionResult> BuscarSugerencias(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new List<object>());

            var resultados = await _context.Entradas
                .Where(e => e.Titulo.Contains(q))
                .OrderByDescending(e => e.Fecha)
                .Select(e => new
                {
                    e.Id,
                    e.Titulo
                })
                .Take(5)
                .ToListAsync();

            return Json(resultados);
        }

        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Index()
        {
            var entradas = await _context.Entradas.Include(e => e.Categoria).ToListAsync();
            return View(entradas);
        }



    }






}



