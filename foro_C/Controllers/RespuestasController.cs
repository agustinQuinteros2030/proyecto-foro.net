using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class RespuestasController : Controller
    {
        private readonly ForoContext _context;

        public RespuestasController(ForoContext context)
        {
            _context = context;
        }

        // GET: Respuestas
        [AllowAnonymous]
        
   
        public async Task<IActionResult> Index()
        {
            var respuestas = await _context.Respuestas
                .AsNoTracking()
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                    .ThenInclude(p => p.Entrada)
                .ToListAsync();

            return View(respuestas);
        }


        // GET: Respuestas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // GET: Respuestas/Create
        [Authorize(Roles = "Miembro,Administrador")]
        public IActionResult Create(int entradaId)
        {
            var pregunta = _context.Preguntas.FirstOrDefault(p => p.EntradaId == entradaId);
            if (pregunta == null)
            {
                return RedirectToAction("Index", "Entradas");
            }

            ViewBag.PreguntaId = pregunta.Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Create([Bind("PreguntaId,Texto")] Respuesta respuesta)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PreguntaId = respuesta.PreguntaId;
                return View(respuesta);
            }

            var userName = User.Identity.Name;
            var miembro = await _context.Miembros.FirstOrDefaultAsync(m => m.UserName == userName);
            if (miembro == null) return Unauthorized();

            respuesta.MiembroId = miembro.Id;
            respuesta.Fecha = DateTime.Now;

            _context.Add(respuesta);
            await _context.SaveChangesAsync();

            var pregunta = await _context.Preguntas.FirstOrDefaultAsync(p => p.Id == respuesta.PreguntaId);
            if (pregunta == null)
            {
                return RedirectToAction("Index", "Entradas");
            }

            return RedirectToAction("Details", "Entradas", new { id = pregunta.EntradaId });
        }
    




        // GET: Respuestas/Edit/5
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int? id)

        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "Id", "Texto", respuesta.PreguntaId);
            return View(respuesta);
        }

        // POST: Respuestas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("PreguntaId,Id,Fecha,Texto,MiembroId")] Respuesta respuesta)
        {
            if (id != respuesta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respuesta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespuestaExists(respuesta.Id))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "Id", "Texto", respuesta.PreguntaId);
            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        [Authorize(Roles = "Miembro,Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Miembro,Administrador")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta != null)
            {
                _context.Respuestas.Remove(respuesta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespuestaExists(int id)
        {
            return _context.Respuestas.Any(e => e.Id == id);
        }
    }
}
