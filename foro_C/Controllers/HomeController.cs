using foro_C.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            // �ltimas 5 entradas
            ViewBag.UltimasEntradas = await _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Categoria)
                .Where(e => !e.Privada)
                .OrderByDescending(e => e.Fecha)
                .Take(5)
                .Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    e.Resumen,
                    MiembroNombre = e.Miembro.Nombre + " " + e.Miembro.Apellido,
                    CategoriaNombre = e.Categoria.Nombre,
                    e.Fecha,
                    CantidadReacciones = e.Preguntas.SelectMany(p => p.Respuestas).SelectMany(r => r.Reacciones).Count(),
                    CantidadComentarios = e.Preguntas.SelectMany(p => p.Respuestas).Count()
                })
                .ToListAsync();

            // Top 5 entradas con m�s preguntas y respuestas
            ViewBag.TopEntradasPregResp = await _context.Entradas
                .Include(e => e.Miembro)
                .Include(e => e.Categoria)
                .Where(e => !e.Privada)
                .OrderByDescending(e => e.Preguntas.Count + e.Preguntas.SelectMany(p => p.Respuestas).Count())
                .Take(5)
                .Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    e.Resumen,
                    MiembroNombre = e.Miembro.Nombre + " " + e.Miembro.Apellido,
                    CategoriaNombre = e.Categoria.Nombre,
                    e.Fecha,
                    CantidadReacciones = e.Preguntas.SelectMany(p => p.Respuestas).SelectMany(r => r.Reacciones).Count(),
                    CantidadComentarios = e.Preguntas.SelectMany(p => p.Respuestas).Count()
                })
                .ToListAsync();

            // Top 3 miembros con m�s entradas en el �ltimo mes
            var haceUnMes = DateTime.Now.AddMonths(-1);
            ViewBag.TopMiembrosMes = await _context.Miembros
                .Select(m => new
                {
                    NombreCompleto = m.Nombre + " " + m.Apellido,
                    CantidadEntradas = m.Entradas.Count(e => e.Fecha >= haceUnMes)
                })
                .OrderByDescending(m => m.CantidadEntradas)
                .Take(3)
                .ToListAsync();

            ViewBag.Categorias = await _context.Categorias.ToListAsync();
            ViewBag.TopMiembros = await _context.Miembros
                .OrderBy(m => m.Nombre)
                .Take(5)
                .ToListAsync();

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}