using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foro2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Foro2._0.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<Persona> _userManager;
        private SignInManager<Persona> _signInManager;
        private ForoContext _context;
        private readonly RoleManager<Rol> _roleManager;

        public HomeController(UserManager<Persona> userManager, SignInManager<Persona> signInManager, ForoContext context, RoleManager<Rol> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            // Últimas 5 entradas ordenadas por fecha descendente
            var entradasRecientes = _context.Entradas
                .Include(e => e.Miembro)
                .OrderByDescending(e => e.FechaCreacion)
                .Take(5)
                .ToList();

            // Top 5 entradas con más preguntas + respuestas (sumadas)
            var topEntradas = _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                .AsEnumerable()
                .OrderByDescending(e => e.Preguntas.Sum(p => p.Respuestas.Count) + e.Preguntas.Count)
                .Take(5)
                .ToList();

            // Top 3 miembros con más entradas en el último mes
            var ultimoMes = DateTime.Now.AddMonths(-1);

            var topMiembros = _context.Entradas
                .Where(e => e.FechaCreacion >= ultimoMes)
                .GroupBy(e => e.Miembro)
                .Select(g => new
                {
                    Miembro = g.Key,
                    CantidadEntradas = g.Count()
                })
                .OrderByDescending(x => x.CantidadEntradas)
                .Take(3)
                .ToList();

            // Pasamos todo a ViewBag
            ViewBag.EntradasRecientes = entradasRecientes;
            ViewBag.TopEntradas = topEntradas;
            ViewBag.TopMiembros = topMiembros;

            // **IMPORTANTE** Pasamos el modelo principal para que la vista no explote en Model.Any()
            // Por ejemplo, todas las entradas para listado general (puedes cambiar filtro si querés)
            var todasEntradas = _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Categoria)
                .OrderByDescending(e => e.FechaCreacion)
                .ToList();

            return View(todasEntradas);
        }



        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
