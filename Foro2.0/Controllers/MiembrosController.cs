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
using System.Threading;

namespace Foro2._0.Controllers
{
    public class MiembrosController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;

        public MiembrosController(ForoContext context, UserManager<Persona> userManager, SignInManager<Persona> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "MIEMBRO")]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Perfil(int id)
        {
            // Obtener el usuario logueado
            var miembro = await _context.Personas
        .FirstOrDefaultAsync(p => p.Id == id);

            if (miembro == null || !(miembro is Miembro))
            {
                return NotFound("Usuario no encontrado o no es un miembro.");
            }

            // Traer al miembro con todas sus colecciones relacionadas
             miembro = await _context.Miembros
                .Include(m => m.EntradasCreadas)
                .ThenInclude(e => e.Categoria)
                    .Include(m => m.PreguntasRealizadas)
        .Include(m => m.RespuestasRealizadas)
        .Include(m => m.ReaccionesRealizadas)
        .Include(m => m.AccesoEntradasPrivadas)
                .FirstOrDefaultAsync(m => m.Id == miembro.Id);


            if (miembro == null)
            {
                return NotFound("No se encontró la información del miembro.");
            }

            return View(miembro);
        }





        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var miembro = await _context.Miembros.FindAsync(user.Id);
            if (miembro == null)
                return NotFound();

            return View(miembro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Edit([Bind("Id,UserName,Nombre,Apellido,Telefono,FechaAlta,Email")] Miembro miembro)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.Id != miembro.Id)
                return Forbid(); // Evita que edite a otro

            if (ModelState.IsValid)
            {
                var usuarioOriginal = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == miembro.Id);
                if (usuarioOriginal == null)
                    return NotFound();

                // Validar si el nuevo username ya existe y no es el suyo
                var userExistente = await _userManager.FindByNameAsync(miembro.UserName);
                if (userExistente != null && userExistente.Id != miembro.Id)
                {
                    ModelState.AddModelError("UserName", "Ese nombre de usuario ya está en uso.");
                    return View(miembro);
                }

                // Actualizar propiedades
                usuarioOriginal.UserName = miembro.UserName;
                usuarioOriginal.Nombre = miembro.Nombre;
                usuarioOriginal.Apellido = miembro.Apellido;
                usuarioOriginal.Telefono = miembro.Telefono;
                usuarioOriginal.Email = miembro.Email;

                var resultado = await _userManager.UpdateAsync(usuarioOriginal);
                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(miembro);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(miembro);
        }


        // GET: Miembros/Delete/5
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var miembro = await _context.Miembros.FindAsync(user.Id);
            if (miembro == null)
                return NotFound();

            return View(miembro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MIEMBRO")]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var miembro = await _context.Miembros.FindAsync(user.Id);
            if (miembro == null)
                return NotFound();

            _context.Miembros.Remove(miembro);
            await _context.SaveChangesAsync();

            await _signInManager.SignOutAsync(); // Cerramos sesión automáticamente
            return RedirectToAction("Index", "Home");
        }


        private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.Id == id);
        }

        [Authorize]
        [HttpGet("Perfil/Actividad")]
        public async Task<IActionResult> Actividad()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("IniciarSesion", "Account");

            var persona = user as Persona;
      

            var miembro = await _context.Miembros
                .Include(m => m.PreguntasRealizadas)
                .Include(m => m.RespuestasRealizadas)
                    .ThenInclude(r => r.Pregunta)
                .Include(m => m.ReaccionesRealizadas)
                    .ThenInclude(r => r.Respuesta)
                .Include(m => m.EntradasCreadas)
                .FirstOrDefaultAsync(m => m.Id == persona.Id);

            if (miembro == null) return NotFound("Miembro no encontrado en DB");

            return View("Actividad", miembro);

        }

    }



}
