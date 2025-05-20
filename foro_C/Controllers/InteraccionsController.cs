using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using foro_C.Data;

namespace foro_C.Controllers
{
    public class InteraccionsController : Controller
    {
        private readonly ForoContext _context;

        public InteraccionsController(ForoContext context)
        {
            _context = context;
        }

        // GET: Interaccions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Interaccion.ToListAsync());
        }

        // GET: Interaccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaccion = await _context.Interaccion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interaccion == null)
            {
                return NotFound();
            }

            return View(interaccion);
        }

        // GET: Interaccions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Interaccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Texto,MiembroId")] Interaccion interaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interaccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(interaccion);
        }

        // GET: Interaccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaccion = await _context.Interaccion.FindAsync(id);
            if (interaccion == null)
            {
                return NotFound();
            }
            return View(interaccion);
        }

        // POST: Interaccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Texto,MiembroId")] Interaccion interaccion)
        {
            if (id != interaccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InteraccionExists(interaccion.Id))
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
            return View(interaccion);
        }

        // GET: Interaccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaccion = await _context.Interaccion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interaccion == null)
            {
                return NotFound();
            }

            return View(interaccion);
        }

        // POST: Interaccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interaccion = await _context.Interaccion.FindAsync(id);
            if (interaccion != null)
            {
                _context.Interaccion.Remove(interaccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InteraccionExists(int id)
        {
            return _context.Interaccion.Any(e => e.Id == id);
        }
    }
}
