using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foro2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Foro2._0.Controllers
{
    [Authorize] // Para que solo usuarios autenticados accedan (modificalo si querés)
    public class RespuestasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public RespuestasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Respuestas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var respuestas = _context.Respuestas.Include(r => r.Pregunta);
            return View(await respuestas.ToListAsync());
        }

        // GET: Respuestas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var respuesta = await _context.Respuestas
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (respuesta == null) return NotFound();

            return View(respuesta);
        }

        // GET: Respuestas/Create
        [Authorize(Roles = "MIEMBRO")]
  
        public async Task<IActionResult> Create(int preguntaId, string returnUrl = null)
        {
            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .FirstOrDefaultAsync(p => p.Id == preguntaId);

            if (pregunta == null) return NotFound();

            var respuesta = new Respuesta
            {
                PreguntaId = pregunta.Id,
                Pregunta = pregunta
            };

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Details", "Entradas", new { id = pregunta.EntradaId });
            return View(respuesta);
        }


        // POST: Respuestas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Create(Respuesta respuesta, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user is Miembro miembro)
                {
                    // Forzar que MiembroId sea del usuario autenticado (ignorar cualquier valor enviado)
                    respuesta.MiembroId = miembro.Id;
                    respuesta.Fecha = DateTime.Now;

                    _context.Add(respuesta);
                    await _context.SaveChangesAsync();

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        var pregunta = await _context.Preguntas.FindAsync(respuesta.PreguntaId);
                        return RedirectToAction("Details", "Entradas", new { id = pregunta?.EntradaId ?? 1 });
                    }

                 

                    return Redirect(returnUrl);
                }

                return Forbid();
            }

            var preguntaText = await _context.Preguntas
                .Where(p => p.Id == respuesta.PreguntaId)
                .Select(p => p.Texto)
                .FirstOrDefaultAsync();

            ViewBag.PreguntaTexto = preguntaText;
            ViewBag.ReturnUrl = returnUrl;

            return View(respuesta);
        }



        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (respuesta.MiembroId != user.Id)
                return Forbid();

            var preguntas = await _context.Preguntas.ToListAsync();
            ViewBag.PreguntaId = new SelectList(preguntas, "Id", "Titulo", respuesta.PreguntaId);

            return View(respuesta);
        }






        // POST: Respuestas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Texto,PreguntaId,Fecha,MiembroId")] Respuesta respuesta)
        {
            if (id != respuesta.Id) return NotFound();

            var original = await _context.Respuestas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            var user = await _userManager.GetUserAsync(User);
            if (original == null || original.MiembroId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respuesta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespuestaExists(respuesta.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var preguntas = await _context.Preguntas.ToListAsync();
            ViewBag.PreguntaId = new SelectList(preguntas, "Id", "Titulo", respuesta.PreguntaId);

            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var respuesta = await _context.Respuestas
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.Id == id);

            var user = await _userManager.GetUserAsync(User);
            if (respuesta == null || respuesta.MiembroId != user.Id)
                return Forbid();

            return View(respuesta);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respuesta = await _context.Respuestas.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (respuesta == null || respuesta.MiembroId != user.Id)
                return Forbid();

            _context.Respuestas.Remove(respuesta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool RespuestaExists(int id)
        {
            return _context.Respuestas.Any(e => e.Id == id);
        }
    }
}

