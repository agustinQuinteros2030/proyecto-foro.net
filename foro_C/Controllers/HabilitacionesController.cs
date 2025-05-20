using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using foro_C.Data;
using foro_C.Models;

namespace foro_C.Controllers
{
    public class HabilitacionesController : Controller
    {
        private readonly ForoContext _context;

        public HabilitacionesController(ForoContext context)
        {
            _context = context;
        }

        // GET: Habilitaciones
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Habilitaciones.Include(h => h.Entrada).Include(h => h.Miembro);
            return View(await foroContext.ToListAsync());
        }

        // GET: Habilitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habilitacion = await _context.Habilitaciones
                .Include(h => h.Entrada)
                .Include(h => h.Miembro)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (habilitacion == null)
            {
                return NotFound();
            }

            return View(habilitacion);
        }

        // GET: Habilitaciones/Create
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto");
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido");
            return View();
        }

        // POST: Habilitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IsHabilitado,MiembroId,EntradaId")] Habilitacion habilitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habilitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", habilitacion.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", habilitacion.MiembroId);
            return View(habilitacion);
        }

        // GET: Habilitaciones/Edit/5
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", habilitacion.MiembroId);
            return View(habilitacion);
        }

        // POST: Habilitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IsHabilitado,MiembroId,EntradaId")] Habilitacion habilitacion)
        {
            if (id != habilitacion.MiembroId)
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
                    if (!HabilitacionExists(habilitacion.MiembroId))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", habilitacion.MiembroId);
            return View(habilitacion);
        }

        // GET: Habilitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habilitacion = await _context.Habilitaciones
                .Include(h => h.Entrada)
                .Include(h => h.Miembro)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (habilitacion == null)
            {
                return NotFound();
            }

            return View(habilitacion);
        }

        // POST: Habilitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
            return _context.Habilitaciones.Any(e => e.MiembroId == id);
        }
    }
}
