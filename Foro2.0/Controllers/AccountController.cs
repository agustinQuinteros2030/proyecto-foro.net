using Foro2._0.Models;
using Foro2._0.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Foro2._0.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<Persona> _userManager;
        private SignInManager<Persona> _signInManager;
        private ForoContext _context;
        private readonly RoleManager<Rol> _roleManager;

        public AccountController(UserManager<Persona> userManager, SignInManager<Persona> signInManager, ForoContext context, RoleManager<Rol> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }
        public IActionResult RegistrarMiembro()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarMiembro(RegistroUsuario model)
        {
            if (ModelState.IsValid)
            {
                var user = new Miembro
                {
                    UserName = model.Username,     // Identity lo necesita para loguear
                
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaAlta = DateTime.Now
                };



                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "MIEMBRO");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        [ActionName("Login")]
        public IActionResult IniciarSesion(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("IniciarSesion");
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        public async Task<IActionResult> IniciarSesion(InicioSesion model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.EmailOUsername, model.Password, model.Recordarme, false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }
          

            ModelState.AddModelError("", "Inicio de sesión inválido");
            ViewBag.ReturnUrl = returnUrl;
            return View("IniciarSesion", model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


    }



   
    }
