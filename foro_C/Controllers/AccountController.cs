using foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace foro_C.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Persona> _usermanager;
        public AccountController(UserManager<Persona> usermanager)
        {
            this._usermanager = usermanager;
        }



        public IActionResult Registrar()
        {
            return View();
        }
    }
}