using foro_C.Data;
using foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    [Authorize(Roles = "Administrador,Miembro")]
    public class CategoriasController : Controller
    {
        private readonly ForoContext _context;
        private readonly SignInManager<Persona> _signInManager;

        public CategoriasController(ForoContext context, SignInManager<Persona> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }



        // GET: Categorias
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.EstaAutenticado = _signInManager.IsSignedIn(User);
            return View(await _context.Categorias.ToListAsync()); ;
        }

        // GET: Categorias/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias
                                          .Include(c => c.Entradas)   // si querés mostrar cuántas entradas tiene
                                          .FirstOrDefaultAsync(c => c.Id == id);

            return categoria is null ? NotFound() : View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            ViewBag.CategoriasExistentes = _context.Categorias.OrderBy(c => c.Nombre).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre")] Categoria categoria)
        {
            var nombreNuevo = new string(categoria.Nombre
                .Where(char.IsLetterOrDigit)
                .ToArray())
                .ToLowerInvariant();

            var categoriasExistentes = _context.Categorias
                .AsEnumerable()
                .Select(c => new string(c.Nombre.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant())
                .ToList();

            // Considera "parecida" si la distancia es 2 o menos (ajusta el umbral si quieres)
            bool esParecida = categoriasExistentes.Any(nombreExistente =>
                LevenshteinDistance(nombreNuevo, nombreExistente) <= 2
            );

            if (esParecida)
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría igual o muy parecida.");
                ViewBag.CategoriasExistentes = _context.Categorias.OrderBy(c => c.Nombre).ToList();
                return View(categoria);
            }

            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();
            if (!ModelState.IsValid) return View(categoria);

            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias
                                          .FirstOrDefaultAsync(c => c.Id == id);
            return categoria is null ? NotFound() : View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria is not null) _context.Categorias.Remove(categoria);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CategoriaExists(int id) =>
            _context.Categorias.Any(e => e.Id == id);

        private int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return target?.Length ?? 0;
            if (string.IsNullOrEmpty(target)) return source.Length;

            var sourceLength = source.Length;
            var targetLength = target.Length;
            var matrix = new int[sourceLength + 1, targetLength + 1];

            for (var i = 0; i <= sourceLength; i++) matrix[i, 0] = i;
            for (var j = 0; j <= targetLength; j++) matrix[0, j] = j;

            for (var i = 1; i <= sourceLength; i++)
            {
                for (var j = 1; j <= targetLength; j++)
                {
                    var cost = source[i - 1] == target[j - 1] ? 0 : 1;
                    matrix[i, j] = new[]
                    {
                        matrix[i - 1, j] + 1,
                        matrix[i, j - 1] + 1,
                        matrix[i - 1, j - 1] + cost
                    }.Min();
                }
            }

            return matrix[sourceLength, targetLength];
        }
    }
}

