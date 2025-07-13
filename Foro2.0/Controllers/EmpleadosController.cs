using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foro2._0.Models;
using Microsoft.AspNetCore.Authorization;

namespace Foro2._0.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ForoContext _context;

        public EmpleadosController(ForoContext context)
        {
            _context = context;
        }

        // GET: Empleados
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Empleados.ToListAsync();
            return View(empleados);
        }


        // GET: Empleados/Details/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = "ADMINISTRADOR")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Create([Bind("Legajo,Nombre,Apellido,Telefono,FechaAlta,Email")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                // Acá podrías agregar lógica para generar username o password automática si aplica

                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }


        // GET: Empleados/Edit/5
       

        // GET: Empleados/Delete/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
