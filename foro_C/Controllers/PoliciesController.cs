using Microsoft.AspNetCore.Mvc;

namespace foro_C.Controllers
{
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

