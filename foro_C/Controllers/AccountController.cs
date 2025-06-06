using foro_C.Helpers;
using foro_C.Models;
using foro_C.ViewsModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("Nombre,Apellido,UserName,Email,Password,ConfirmPassword,Telefono")] RegistroUsuario viewModel)
        {
            if (ModelState.IsValid)
            {
                var emailExists = await _usermanager.Users
                    .AsNoTracking()
                    .AnyAsync(u => u.Email == viewModel.Email);

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "El email ya está registrado.");
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
                        return RedirectToAction("Edit", "Miembros", new { id = miembroACrear.Id }); // Redirigir a la página de inicio o a donde desees
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


        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(IniciarSesion viewModel)
        {
            if (ModelState.IsValid)
            {

                var resultado = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.Recordarme, false);

                if (resultado.Succeeded)
                {

                    // Si el inicio de sesion es exitoso
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de inicio de sesion no valido. Por favor, verifique sus credenciales.");
                }

            }
            return View(viewModel);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
