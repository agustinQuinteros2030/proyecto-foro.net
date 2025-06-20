using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace foro_C.Controllers
{
    [AllowAnonymous]
    public class PoliciesController : Controller
    {
        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserAgreement()
        {
            return View();
        }
    }
}

