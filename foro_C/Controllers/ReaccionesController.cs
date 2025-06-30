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
    [Authorize]
    public class ReaccionesController : Controller

    {
        private readonly ForoContext _context;

        public ReaccionesController(ForoContext context)
        {
            _context = context;
        }

        // GET: Reacciones
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Reacciones.Include(r => r.Miembro).Include(r => r.Respuesta);
            return View(await foroContext.ToListAsync());
        }

        // GET: Reacciones/Details/5
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // GET: Reacciones/Create
        [Authorize(Roles = "Miembro,Administrador")]
        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido");
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Texto");
            return View();
        }

        // POST: Reacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Create([Bind("Tipo,RespuestaId,Id,Texto,MiembroId")] Reaccion reaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reaccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Texto", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Edit/5
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reaccion == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (reaccion.MiembroId != userId)
                return Forbid(); // o RedirectToAction("Index")

            return View(reaccion);
        }
        

        // POST: Reacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]

        public async Task<IActionResult> Edit(int id, [Bind("Tipo,RespuestaId,Id,Fecha,Texto")] Reaccion reaccion)
        {
            if (id != reaccion.Id)
                return NotFound();

            var reaccionOriginal = await _context.Reacciones.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reaccionOriginal == null)
                return NotFound();

            // Seguridad: solo el autor puede editar
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (reaccionOriginal.MiembroId != userId)
                return Forbid();

            // Preservamos campos que no deben cambiar
            reaccion.MiembroId = reaccionOriginal.MiembroId;
            reaccion.Fecha = reaccionOriginal.Fecha;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reaccion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaccionExists(reaccion.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // No volvemos a mostrar dropdowns si son innecesarios
            return View(reaccion);
        }


        // GET: Reacciones/Delete/5
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // POST: Reacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reaccion = await _context.Reacciones.FindAsync(id);
            if (reaccion != null)
            {
                _context.Reacciones.Remove(reaccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaccionExists(int id)
        {
            return _context.Reacciones.Any(e => e.Id == id);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reaccionar(int entradaId, string accion)
        {
            var userName = User.Identity.Name;
            var miembro = await _context.Miembros.FirstOrDefaultAsync(m => m.UserName == userName);
            if (miembro == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction("Details", "Entradas", new { id = entradaId });
            }

            var entrada = await _context.Entradas
                .Include(e => e.Miembro)
                .FirstOrDefaultAsync(e => e.Id == entradaId);
            if (entrada == null)
            {
                TempData["Error"] = "Entrada no encontrada.";
                return RedirectToAction("Index", "Entradas");
            }

            if (entrada.Privada || !entrada.Activa || entrada.Estado != EstadoEntrada.Publicada)
            {
                TempData["Error"] = "No puedes reaccionar a esta entrada.";
                return RedirectToAction("Details", "Entradas", new { id = entradaId });
            }

            var reaccion = await _context.Reacciones
                .FirstOrDefaultAsync(r => r.MiembroId == miembro.Id && r.RespuestaId == entradaId); // <- revisá si debería ser EntradaId

            if (accion == "agregar")
            {
                if (reaccion == null)
                {
                    reaccion = new Reaccion
                    {
                        MiembroId = miembro.Id,
                        RespuestaId = entradaId, // o EntradaId si corresponde
                        Tipo = TipoReaccion.MeGusta
                    };
                    _context.Reacciones.Add(reaccion);
                    await _context.SaveChangesAsync();
                }
            }
            else if (accion == "quitar")
            {
                if (reaccion != null)
                {
                    _context.Reacciones.Remove(reaccion);
                    await _context.SaveChangesAsync();
                }
            }

            // Redirigimos de vuelta a la vista de detalles
            return RedirectToAction("Details", "Entradas", new { id = entradaId });
        }
    }
}
