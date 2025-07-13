using Foro2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Foro2._0.Controllers
{
    public class ReaccionesController : Controller
    {

        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public ReaccionesController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Create(int respuestaId, TipoReaccion tipo, string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            // Buscamos reacción existente solo para este usuario en esta respuesta
            var reaccionExistente = await _context.Reacciones
                .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == user.Id);

            if (reaccionExistente != null)
            {
                // Validamos que la reacción realmente pertenezca a este usuario (extra protección)
                if (reaccionExistente.MiembroId != user.Id)
                {
                    return Forbid(); // No puede modificar reacciones de otros
                }

                if (reaccionExistente.Tipo == tipo)
                {
                    _context.Reacciones.Remove(reaccionExistente);
                }
                else
                {
                    reaccionExistente.Tipo = tipo;
                    reaccionExistente.Fecha = DateTime.Now;
                    _context.Reacciones.Update(reaccionExistente);
                }
            }
            else
            {
                // Crear nueva reacción, siempre asignando el MiembroId actual para evitar suplantación
                var reaccion = new Reaccion
                {
                    Fecha = DateTime.Now,
                    RespuestaId = respuestaId,
                    Tipo = tipo,
                    MiembroId = user.Id
                };
                _context.Add(reaccion);
            }

            await _context.SaveChangesAsync();

            return Redirect(returnUrl ?? Url.Action("Index", "Entradas"));
        }


        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reaccion == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (reaccion.MiembroId != user.Id)
                return Forbid();

            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Texto", reaccion.RespuestaId);
            return View(reaccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo,RespuestaId,Fecha,MiembroId")] Reaccion reaccion)
        {
            if (id != reaccion.Id) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var original = await _context.Reacciones.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

            if (original == null || original.MiembroId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Reacciones.Any(e => e.Id == reaccion.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Texto", reaccion.RespuestaId);
            return View(reaccion);
        }

        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reaccion == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (reaccion.MiembroId != user.Id)
                return Forbid();

            return View(reaccion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reaccion = await _context.Reacciones.FindAsync(id);
            if (reaccion == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (reaccion.MiembroId != user.Id)
                return Forbid();

            _context.Reacciones.Remove(reaccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ReaccionesExists(int id)
        {
            return _context.Reacciones.Any(e => e.Id == id);
        }

    }
}
