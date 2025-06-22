using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    [Authorize(Roles = "Administrador,Miembro")]
    public class PreguntasController : Controller
    {
        private readonly ForoContext _context;

        public PreguntasController(ForoContext context)
        {
            _context = context;
        }

        // GET: Preguntas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Preguntas.Include(p => p.Entrada).Include(p => p.Miembro);
            return View(await foroContext.ToListAsync());
        }

        // GET: Preguntas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // GET: Preguntas/Create
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto");
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido");
            return View();
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Activa,EntradaId,Id,Texto")] Pregunta pregunta)
        {
            // Obtén el usuario actual
            var userName = User.Identity.Name;
            var miembro = await _context.Miembros.FirstOrDefaultAsync(m => m.UserName == userName);
            if (miembro == null)
                return Unauthorized();

            pregunta.MiembroId = miembro.Id; // Asigna el miembro autenticado

            if (ModelState.IsValid)
            {
                _context.Add(pregunta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", pregunta.EntradaId);
            return View(pregunta);
        }

        // GET: Preguntas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // POST: Preguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Activa,EntradaId,Id,Fecha,Texto,MiembroId")] Pregunta pregunta)
        {
            if (id != pregunta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregunta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreguntaExists(pregunta.Id))
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
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // GET: Preguntas/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // POST: Preguntas/Activar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar(int id)
        {
            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            pregunta.Activa = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // POST: Preguntas/Desactivar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Miembro")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            // Solo el dueño o un admin puede desactivar
            var currentUserId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);
            if (pregunta.MiembroId != currentUserId && !User.IsInRole("Administrador"))
                return Forbid();

            pregunta.Activa = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta != null)
            {
                _context.Preguntas.Remove(pregunta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreguntaExists(int id)
        {
            return _context.Preguntas.Any(e => e.Id == id);
        }
    }

}
