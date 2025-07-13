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
    public class PreguntasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public PreguntasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Preguntas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Preguntas.Include(p => p.Entrada);
            return View(await foroContext.ToListAsync());
        }

        // GET: Preguntas/Details/
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // GET: Preguntas/Create
        [Authorize(Roles = "MIEMBRO")]

       
        public async Task<IActionResult> Create(int entradaId, string returnUrl = null)
        {
            var entrada = await _context.Entradas.FindAsync(entradaId);
            if (entrada == null) return NotFound();

            var pregunta = new Pregunta
            {
                EntradaId = entradaId,
                Entrada = entrada
            };

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Details", "Entradas", new { id = entradaId });
            return View(pregunta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Create(Pregunta pregunta, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user is Miembro miembro)
                {
                    pregunta.MiembroId = miembro.Id;
                    pregunta.Fecha = DateTime.Now;

                    _context.Preguntas.Add(pregunta);
                    await _context.SaveChangesAsync();

                    return Redirect(returnUrl ?? Url.Action("Details", "Entradas", new { id = pregunta.EntradaId }));
                }

                return Forbid();
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(pregunta);
        }



        // GET: Preguntas/Edit/5
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || pregunta.MiembroId != user.Id)
                return Forbid(); // ❌ no es dueño de la pregunta

            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", pregunta.EntradaId);
            return View(pregunta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit(int id, [Bind("Texto,Activa,EntradaId,Id,Fecha,MiembroId")] Pregunta pregunta)
        {
            if (id != pregunta.Id)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || pregunta.MiembroId != user.Id)
                return Forbid(); // ❌ intento editar pregunta ajena

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
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Texto", pregunta.EntradaId);
            return View(pregunta);
        }

        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pregunta == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || pregunta.MiembroId != user.Id)
                return Forbid(); // ❌ intento de otro miembro

            return View(pregunta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var pregunta = await _context.Preguntas.FindAsync(id);

            if (pregunta == null)
                return NotFound();

            if (pregunta.MiembroId != user.Id)
                return Forbid(); // ❌ intento de borrar pregunta ajena

            _context.Preguntas.Remove(pregunta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private bool PreguntaExists(int id)
        {
            return _context.Preguntas.Any(e => e.Id == id);
        }
    }
}
