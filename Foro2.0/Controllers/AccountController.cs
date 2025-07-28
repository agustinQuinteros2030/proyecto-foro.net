using Foro2._0.Models;
using Foro2._0.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Foro2._0.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<Persona> _userManager;
        private SignInManager<Persona> _signInManager;
        private ForoContext _context;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IHostEnvironment _environment;

        public AccountController(UserManager<Persona> userManager, SignInManager<Persona> signInManager, ForoContext context, RoleManager<Rol> roleManager, IHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _environment = environment;
        }
        public IActionResult RegistrarMiembro()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarMiembro(RegistroUsuario model, IFormFile? ImagenPerfil)
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

                // Subida de imagen de perfil (Nueva funcionalidad)
                if (ImagenPerfil != null && ImagenPerfil.Length > 0)
                {
                    // Corrected line to fix syntax and declaration issues
                    var uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "uploads", "perfiles");

                    // Create the directory if it does not exist
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenPerfil.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenPerfil.CopyToAsync(fileStream);
                    }

                    user.ImagenPerfilRuta = "/uploads/perfiles/" + uniqueFileName;
                }

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
