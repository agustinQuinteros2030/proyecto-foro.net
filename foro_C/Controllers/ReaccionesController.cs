using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            if (id == null)
            {
                return NotFound();
            }

            var reaccion = await _context.Reacciones.FindAsync(id);
            if (reaccion == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Texto", reaccion.RespuestaId);
            return View(reaccion);
        }

        // POST: Reacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Tipo,RespuestaId,Id,Fecha,Texto,MiembroId")] Reaccion reaccion)
        {
            if (id != reaccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaccionExists(reaccion.Id))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Texto", reaccion.RespuestaId);
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
            // Obtener el usuario actual
            var userName = User.Identity.Name;
            var miembro = await _context.Miembros.FirstOrDefaultAsync(m => m.UserName == userName);
            if (miembro == null)
                return Json(new { success = false, mensaje = "Usuario no encontrado" });

            // Obtener la entrada
            var entrada = await _context.Entradas.Include(e => e.Miembro).FirstOrDefaultAsync(e => e.Id == entradaId);
            if (entrada == null)
                return Json(new { success = false, mensaje = "Entrada no encontrada" });

            // Validar si la entrada es privada o está deshabilitada
            if (entrada.Privada)
                return Json(new { success = false, mensaje = "No puedes reaccionar a una entrada privada." });

            if (!entrada.Activa || entrada.Estado != EstadoEntrada.Publicada)
                return Json(new { success = false, mensaje = "No puedes reaccionar a una entrada deshabilitada." });

            // Buscar si ya existe una reacción de este usuario para esta entrada
            var reaccion = await _context.Reacciones
                .FirstOrDefaultAsync(r => r.MiembroId == miembro.Id && r.RespuestaId == entradaId);

            if (accion == "agregar")
            {
                if (reaccion == null)
                {
                    reaccion = new Reaccion
                    {
                        MiembroId = miembro.Id,
                        RespuestaId = entradaId, // Ajusta si tienes una propiedad específica para EntradaId
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

            var nuevaCantidad = await _context.Reacciones.CountAsync(r => r.RespuestaId == entradaId);

            return Json(new { success = true, nuevaCantidad });
        }
    }
}
