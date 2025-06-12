using foro_C.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Interaccion.ToListAsync());
        }
        [AllowAnonymous]

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
        [Authorize(Roles = "Miembro")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Interaccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Texto,MiembroId")] Interaccion interaccion)
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
        [Authorize(Roles = "Miembro")]
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
        [Authorize(Roles = "Miembro")]
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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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
