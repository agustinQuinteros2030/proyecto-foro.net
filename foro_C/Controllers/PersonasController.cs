using foro_C.Data;
using foro_C.Helpers;
using foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public PersonasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personas.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(bool EsAdmin, [Bind("Id,UserName,Nombre,Apellido,Email,DireccionID,Telefono")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(persona);
                //await _context.SaveChangesAsync();


                var resultadoNewPersona = await _userManager.CreateAsync(persona, Confings.PasswordGenerica);
                if (resultadoNewPersona.Succeeded)
                {
                    IdentityResult resultadoAddRole;
                    string RolDefinido;
                    //agregamos el rol correspondiente
                    if (EsAdmin)
                    {
                        //agrego el rol de administrador
                        RolDefinido = Confings.AdminRole;

                    }
                    else
                    {

                        //agrego el rol de usuario normal

                        RolDefinido = Confings.MiembroRole;

                    }

                    resultadoAddRole = await _userManager.AddToRoleAsync(persona, RolDefinido);

                    if (resultadoAddRole.Succeeded)
                    {
                        //todo ok, redirigimos a la lista de personas
                        _context.Add(persona);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        return Content($"Error al agregar el rol {RolDefinido} a la persona {persona.UserName}");
                    }

                }


                // procesamos los errores si es que hubo
                foreach (var error in resultadoNewPersona.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Nombre,Apellido,FechaAlta,Email,DireccionID,Telefono")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.Id == id);
        }
    }
}
