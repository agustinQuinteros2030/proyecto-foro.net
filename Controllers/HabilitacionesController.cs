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
    public class HabilitacionesController : Controller
    {
        private readonly ForoContext _context;
   
        private readonly UserManager<Persona> _userManager;

        public HabilitacionesController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
       
        public async Task<IActionResult> Solicitar(int entradaId)
        {
            var user = await _userManager.GetUserAsync(User);

            // Evitar duplicados
            var yaExiste = await _context.Habilitaciones
                .AnyAsync(h => h.EntradaId == entradaId && h.MiembroId == user.Id);

            if (!yaExiste)
            {
                var solicitud = new Habilitacion
                {
                    EntradaId = entradaId,
                    MiembroId = user.Id,
                    Habilitado = false // Estado inicial: solicitud pendiente
                };

                _context.Habilitaciones.Add(solicitud);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Entradas", new { id = entradaId });
        }




        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Solicitudes(int entradaId)
        {
            var user = await _userManager.GetUserAsync(User);
            var entrada = await _context.Entradas.Include(e => e.Miembro).FirstOrDefaultAsync(e => e.Id == entradaId);

            if (entrada == null || entrada.AutorId != user.Id)
            {
                return Forbid(); // No puede ver si no es el autor
            }

            var solicitudes = await _context.Habilitaciones
                .Include(h => h.Miembro)
                .Where(h => h.EntradaId == entradaId && !h.Habilitado)
                .ToListAsync();

            ViewBag.EntradaTitulo = entrada.Titulo;
            ViewBag.EntradaId = entrada.Id;

            return View(solicitudes);
        }

        [HttpPost]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Aprobar(int entradaId, int miembroId)
        {
            var user = await _userManager.GetUserAsync(User);

            var entrada = await _context.Entradas.FindAsync(entradaId);

            // Seguridad: solo el autor puede aprobar
            if (entrada.AutorId != user.Id)
                return Forbid();

            var habilitacion = await _context.Habilitaciones
                .FirstOrDefaultAsync(h => h.EntradaId == entradaId && h.MiembroId == miembroId);

            if (habilitacion != null)
            {
                habilitacion.Habilitado = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Entradas", new { id = entradaId });
        }





        // GET: Habilitaciones
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Habilitaciones.Include(h => h.Entrada);
            return View(await foroContext.ToListAsync());
        }

        // GET: Habilitaciones/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habilitacion = await _context.Habilitaciones
                .Include(h => h.Entrada)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habilitacion == null)
            {
                return NotFound();
            }

            return View(habilitacion);
        }

        // GET: Habilitaciones/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto");
            return View();
        }

        // POST: Habilitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,EntradaId,MiembroId,Habilitado,FechaSolicitud")] Habilitacion habilitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habilitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", habilitacion.EntradaId);
            return View(habilitacion);
        }

        // GET: Habilitaciones/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habilitacion = await _context.Habilitaciones.FindAsync(id);
            if (habilitacion == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", habilitacion.EntradaId);
            return View(habilitacion);
        }

        // POST: Habilitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EntradaId,MiembroId,Habilitado,FechaSolicitud")] Habilitacion habilitacion)
        {
            if (id != habilitacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habilitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabilitacionExists(habilitacion.Id))
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
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", habilitacion.EntradaId);
            return View(habilitacion);
        }

        // GET: Habilitaciones/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habilitacion = await _context.Habilitaciones
                .Include(h => h.Entrada)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habilitacion == null)
            {
                return NotFound();
            }

            return View(habilitacion);
        }

        // POST: Habilitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habilitacion = await _context.Habilitaciones.FindAsync(id);
            if (habilitacion != null)
            {
                _context.Habilitaciones.Remove(habilitacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabilitacionExists(int id)
        {
            return _context.Habilitaciones.Any(e => e.Id == id);
        }

      




    }
}
