using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AdminController : Controller
{

    private readonly ForoContext _context;
    private readonly RoleManager<Rol> roleManager;
    private readonly UserManager<Persona> userManager;
    private readonly SignInManager<Persona> _signInManager;

    public AdminController(ForoContext context, UserManager<Persona> userManager, RoleManager<Rol> roleManager, SignInManager<Persona> signInManager)
    {
        _context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
        _signInManager = signInManager;
    }



    public IActionResult PanelAdmin()
    {
        return View();
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> MiPerfil()
    {
        var usuarioId = userManager.GetUserId(User); // string
        var empleado = await _context.Empleados
      
            .FirstOrDefaultAsync(e => e.Id == int.Parse(usuarioId));

        if (empleado == null) return NotFound();

        return View(empleado);
    }


}