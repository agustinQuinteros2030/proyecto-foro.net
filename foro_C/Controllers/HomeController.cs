using foro_C.Data; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class HomeController : Controller
    {
        private readonly ForoContext _context;

        public HomeController(ForoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var entradas = await _context.Entradas
                .Where(e => e.Activa && !e.Privada)
                .Include(e => e.Miembro)
                .Include(e => e.Categoria)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();

            return View(entradas); // Le manda la lista a la vista Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
