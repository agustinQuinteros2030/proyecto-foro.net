
using foro_C.Models;
using foro_C.ViewsModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Registrar([Bind("Email,Password,ConfirmPassword")] RegistroUsuario viewModel)
        {
            if (ModelState.IsValid)
            {
                // avanzamos con el registro
                Miembro miembroACrear = new Miembro()
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email // Usamos el email como nombre de usuario
                };
                var resultadoCreate = await _usermanager.CreateAsync(miembroACrear, viewModel.Password);

                if (resultadoCreate.Succeeded)
                {
                    await _signInManager.SignInAsync(miembroACrear, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // Redirigir a la página de inicio o a donde desees
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
    }
}
