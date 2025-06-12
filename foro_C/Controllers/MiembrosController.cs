using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    //  [Authorize(Roles = "Administrador")]
    public class MiembrosController : Controller

    {
        private readonly ForoContext _context;

        public MiembrosController(ForoContext context)
        {
            _context = context;
        }

        // GET: Miembros
        // [Authorize(Roles = "Administrador,Miembro")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Miembros.ToListAsync());
        }

        // GET: Miembros/Details/5
        // [Authorize(Roles = "Administrador,Miembro")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // GET: Miembros/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Miembros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,UserName,Nombre,Apellido,Email,Telefono")] Miembro miembro)
        {

            if (ModelState.IsValid)
            {
                _context.Add(miembro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        // GET: Miembros/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro == null)
            {
                return NotFound();
            }
            return View(miembro);
        }

        // POST: Miembros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Nombre,Apellido,FechaAlta,Email,DireccionID,Telefono")] Miembro miembroFormulario)
        {
            if (id != miembroFormulario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var miembroEnDb = await _context.Miembros.FindAsync(id);

                    if (miembroEnDb == null)
                    {
                        return NotFound();
                    }
                    miembroEnDb.Id = miembroFormulario.Id;
                    miembroEnDb.UserName = miembroFormulario.UserName;
                    miembroEnDb.Nombre = miembroFormulario.Nombre;
                    miembroEnDb.Apellido = miembroFormulario.Apellido;
                    miembroEnDb.Telefono = miembroFormulario.Telefono;

                    if (!ActualizarEmail(miembroFormulario, miembroEnDb))
                    {
                        ModelState.AddModelError("Email", "El email ya está en uso por otro usuario.");
                        return View(miembroFormulario);
                    }

                    _context.Update(miembroEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembroFormulario.Id))
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
            return View(miembroFormulario);
        }

        private bool ActualizarEmail(Miembro miembroFormulario, Miembro emailActual)
        {
            bool resultado = true;
            try
            {
                if (!emailActual.NormalizedEmail.Equals(miembroFormulario.Email.ToUpper()))
                {
                    //si no son iguales, actualizamos el email
                    if (ExistEmail(miembroFormulario.Email))
                    {

                        resultado = false; // Email ya existe
                    }
                    else
                    {
                        emailActual.Email = miembroFormulario.Email;
                        emailActual.NormalizedEmail = miembroFormulario.Email.ToUpper();
                    }
                }
                else
                {
                    //si son iguales, no hacemos nada
                    resultado = true; // Email no se actualiza, pero no hay error
                }
            }
            catch
            {
                resultado = false; // Error al actualizar el email
            }
            return resultado;
        }

        private bool ExistEmail(string email)
        {
            return _context.Miembros.Any(m => m.Email == email.ToUpper());
        }

        // GET: Miembros/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro != null)
            {
                _context.Miembros.Remove(miembro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.Id == id);
        }
    }
}
