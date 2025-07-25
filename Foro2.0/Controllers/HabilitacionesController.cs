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
