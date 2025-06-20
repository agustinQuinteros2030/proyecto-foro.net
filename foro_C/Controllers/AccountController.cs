using foro_C.Data;
using foro_C.Helpers;
using foro_C.Models;
using foro_C.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContext _contexto;

        public AccountController(UserManager<Persona> usermanager,
            SignInManager<Persona> signInManager,
            RoleManager<Rol> roleManager,
            ForoContext contexto)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            this._roleManager = roleManager;
            _contexto = contexto;
        }
        [AllowAnonymous]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([Bind("Nombre,Apellido,UserName,Email,Password,ConfirmPassword,Telefono")] RegistroUsuario viewModel)
        {
            if (ModelState.IsValid)
            {
                var emailExists = await _usermanager.Users
                    .AsNoTracking()
                    .AnyAsync(u => u.Email == viewModel.Email);

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "El email ya est� registrado.");
                    return View(viewModel);
                }

                // avanzamos con el registro
                Miembro miembroACrear = new Miembro()
                {
                    Email = viewModel.Email,
                    UserName = viewModel.UserName,
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    Telefono = viewModel.Telefono,
                };
                var resultadoCreate = await _usermanager.CreateAsync(miembroACrear, viewModel.Password);

                if (resultadoCreate.Succeeded)
                {
                    var resultadoAddRole = await _usermanager.AddToRoleAsync(miembroACrear, Confings.MiembroRole);

                    if (resultadoAddRole.Succeeded)
                    {

                        await _signInManager.SignInAsync(miembroACrear, isPersistent: false);
                        return RedirectToAction("Edit", "Miembros", new { id = miembroACrear.Id }); // Redirigir a la p�gina de inicio o a donde desees
                    }
                    else
                    {
                        ModelState.AddModelError(Confings.AdminRole, "Error al asignar el rol al usuario. Por favor, intente nuevamente.");
                    }
                }

                else
                {
                    foreach (var error in resultadoCreate.Errors)
                    {
                        //Prcesamos los errores al crear el usuario
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult IniciarSesion(string returnUrl)
        {

            TempData["Url3"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(IniciarSesion viewModel)
        {
            string returnUrl = TempData["Url3"] as string;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                var resultado = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    return !string.IsNullOrEmpty(returnUrl)
                        ? Redirect(returnUrl)
                        : RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido. Verificá usuario/email y contraseña.");
            return View(viewModel);
        }

           
    



        [Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> listarRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult AccesoDenegado(string returnUrl)
        {
           ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public IActionResult TestCurrentUser()
        {
            if (_signInManager.IsSignedIn(User))
            {
                string nombreUsuario = User.Identity.Name;

                Persona persona = _contexto.Personas
                    .FirstOrDefault(p => p.NormalizedUserName == nombreUsuario.ToUpper());

                int personald = Int32.Parse(_usermanager.GetUserId(User));
                int personald2 = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var personaId3 = User.Claims
                    .FirstOrDefault(c => c.Type == "http://schemas.mlsoap.org/us/2005/05/identity/claims/nameidentifier");
            }

            return null;
        }

    }
}
