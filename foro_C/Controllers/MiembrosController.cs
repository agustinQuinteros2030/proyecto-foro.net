using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    [Authorize(Roles = "Administrador,Miembro")]
    public class MiembrosController : Controller


    {
        private readonly ForoContext _context;
        private readonly RoleManager<Rol> roleManager;
        private readonly UserManager<Persona> userManager;
        private readonly SignInManager<Persona> _signInManager;

        public MiembrosController(ForoContext context, UserManager<Persona> userManager, RoleManager<Rol> roleManager, SignInManager<Persona> signInManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _signInManager = signInManager;
        }

        // GET: Miembros
        [Authorize(Roles = "Administrador,Miembro")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Miembros.ToListAsync());
        }

        // GET: Miembros/Details/5
        [Authorize(Roles = "Administrador,Miembro")]
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Miembros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuarioLogueadoId = userManager.GetUserId(User); // string
            var miembro = await _context.Miembros.FindAsync(id);

            if (miembro == null) return NotFound();

            if (miembro.Id.ToString() != usuarioLogueadoId)
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            return View(miembro);
        }

        // POST: Miembros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]

      
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Nombre,Apellido,FechaAlta,Email,DireccionID,Telefono")] Miembro miembroFormulario)
        {
            if (id != miembroFormulario.Id) return NotFound();

            var usuarioLogueadoId = userManager.GetUserId(User);

            if (miembroFormulario.Id.ToString() != usuarioLogueadoId && !User.IsInRole("Administrador"))
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var miembroEnDb = await _context.Miembros.FindAsync(id);
                    if (miembroEnDb == null) return NotFound();

                    // Actualizar datos
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

                    // 🔄 Actualizar sesión (claims) si cambió UserName o Email
                    if (miembroEnDb.UserName != User.Identity?.Name ||
                        miembroEnDb.Email != User.FindFirstValue(ClaimTypes.Email))
                    {
                        await _signInManager.RefreshSignInAsync(miembroEnDb);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembroFormulario.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(miembroFormulario);
        }


        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros.FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            var usuarioLogueadoId = userManager.GetUserId(User); // string

            // Validar que solo se pueda acceder si sos el dueño del perfil o un admin
            if (miembro.Id.ToString() != usuarioLogueadoId && !User.IsInRole("Administrador"))
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            return View(miembro);
        }


        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (id <= 0) return NotFound();

            var usuarioLogueadoId = userManager.GetUserId(User);
            var miembro = await _context.Miembros.FindAsync(id);

            if (miembro == null) return NotFound();

            // Validamos que el usuario logueado sea el dueño del perfil o admin
            if (miembro.Id.ToString() != usuarioLogueadoId && !User.IsInRole("Administrador"))
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            _context.Miembros.Remove(miembro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.Id == id);
        }


        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {

           
        
            var userId = userManager.GetUserId(User); // devuelve string

            var miembro = await _context.Miembros
                .Include(m => m.Entradas)
                .Include(m => m.Preguntas)
                .ThenInclude(p => p.Entrada)
              .FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);

            if (miembro == null)
                return NotFound();

            return View(miembro);
        }

        [Authorize]

        public async Task<IActionResult> Historial(int id)
        {
            var miembro = await _context.Miembros
      .Include(m => m.Entradas)
      .Include(m => m.Preguntas)
          .ThenInclude(p => p.Entrada)
      .FirstOrDefaultAsync(m => m.Id == id);

            if (miembro == null)
            {
                return NotFound();
            }

            // Verifica si el usuario actual es dueño del perfil o es administrador
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != miembro.Id.ToString() && !User.IsInRole("Administrador"))
            {
                return Forbid(); // o RedirectToAction("AccessDenied", "Account");
            }

            return View(miembro);

        }



    }







}

