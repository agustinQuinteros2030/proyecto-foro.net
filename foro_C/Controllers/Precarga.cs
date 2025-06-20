using foro_C.Data;
using foro_C.Helpers;
using foro_C.Models;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class Precarga1 : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContext _context;
        private readonly ILogger<Precarga1> _logger;

        public Precarga1(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ForoContext context, ILogger<Precarga1> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await InicializarDatosAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task InicializarDatosAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando precarga de datos...");

                if (await _context.Roles.AnyAsync())
                {
                    _logger.LogInformation("Los datos ya están cargados. Saltando precarga.");
                    return;
                }

                await CrearRolesAsync();
                var miembros = await CrearMiembrosAsync();
                var empleados = await CrearAdminsAsync();
                var categorias = await CrearCategoriasAsync();
                var entradas = await CrearEntradasAsync(categorias, miembros);
                var preguntas = await CrearPreguntasAsync(entradas, miembros);
                var respuestas = await CrearRespuestasAsync(preguntas, miembros);
                await CrearReaccionesAsync(respuestas, miembros);
                await CrearHabilitacionesAsync(entradas, miembros);

                _logger.LogInformation("Precarga completada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la precarga.");
            }
        }

        private async Task CrearRolesAsync()
        {
            var roles = new[] { Confings.AdminRole, Confings.MiembroRole };

            foreach (var rol in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rol))
                {
                    await _roleManager.CreateAsync(new Rol(rol));
                }
            }
        }

        private async Task<List<Miembro>> CrearMiembrosAsync()
        {
            var lista = new List<Miembro>
            {
                new() { UserName = "iron.agus", Nombre = "Agustín", Apellido = "Quinteros", Email = "agus@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 10101010 },
                new() { UserName = "gamer.ari", Nombre = "Ariel", Apellido = "Mendoza", Email = "ari@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 20202020 },
                new() { UserName = "chef.lu", Nombre = "Lucía", Apellido = "Paz", Email = "lucia@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 30303030 },
                new() { UserName = "dev.ro", Nombre = "Romina", Apellido = "Torres", Email = "romi@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 40404040 },
                new() { UserName = "bike.franco", Nombre = "Franco", Apellido = "Herrera", Email = "franco@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 50505050 },
                new() { UserName = "anime.manu", Nombre = "Manuel", Apellido = "Saito", Email = "manu@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 60606060 },
                new() { UserName = "musica.ema", Nombre = "Emanuel", Apellido = "Pérez", Email = "ema@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 70707070 },
                new() { UserName = "pwr.caro", Nombre = "Carolina", Apellido = "Benítez", Email = "caro@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 80808080 },
                new() { UserName = "plant.kate", Nombre = "Katherine", Apellido = "Flores", Email = "kate@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 90909090 },
                new() { UserName = "trip.ivan", Nombre = "Iván", Apellido = "Delgado", Email = "ivan@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 11112222 }
            };

            foreach (var m in lista)
            {
                var result = await _userManager.CreateAsync(m, Confings.PasswordGenerica);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(m, Confings.MiembroRole);
            }

            return lista;
        }

        private async Task<List<Empleado>> CrearAdminsAsync()
        {
            var lista = new List<Empleado>
            {
                new() { Legajo = "01EP01", UserName = "empleado1", Nombre = "Empleado", Apellido = "Uno", Email = "empleado1@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 12345679 },
                new() { Legajo = "01EP02", UserName = "empleadorrhh1", Nombre = "Empleado", Apellido = "RRHH", Email = "empleadorrhh1@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 12345680 },
                new() { Legajo = "01EP03", UserName = "empleado2", Nombre = "Empleado", Apellido = "Dos", Email = "empleado2@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 12345681 }
            };

            foreach (var admin in lista)
            {
                var result = await _userManager.CreateAsync(admin, Confings.PasswordGenerica);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, Confings.AdminRole);
            }

            return lista;
        }

        private async Task<List<Categoria>> CrearCategoriasAsync()
        {
            var categorias = new List<Categoria>
            {
                new() { Nombre = "Powerlifting" },
                new() { Nombre = "Videojuegos" },
                new() { Nombre = "Cocina" }
            };

            _context.Categorias.AddRange(categorias);
            await _context.SaveChangesAsync();

            return categorias;
        }

        private async Task<List<Entrada>> CrearEntradasAsync(List<Categoria> categorias, List<Miembro> miembros)
        {
            var entrada1 = new Entrada
            {
                Titulo = "¿Cómo mejorar el press de banca?",
                Texto = "Estoy estancado en mi 1RM de banca hace 3 meses. ¿Qué me recomiendan?",
                Fecha = DateTime.Now,
                Privada = false,
                Categoria = categorias[0],
                Miembro = miembros[0]
            };

            var entrada2 = new Entrada
            {
                Titulo = "Fallout New Vegas: ¿mejor build de sniper?",
                Texto = "Estoy volviendo al juego y quiero probar algo con VATS y sigilo.",
                Fecha = DateTime.Now,
                Privada = true,
                Categoria = categorias[1],
                Miembro = miembros[1]
            };

            var entrada3 = new Entrada
            {
                Titulo = "¿Cómo hago tofu crocante como en los restaurantes?",
                Texto = "Intenté mil veces pero siempre me queda blando o gomoso.",
                Fecha = DateTime.Now,
                Privada = false,
                Categoria = categorias[2],
                Miembro = miembros[2]
            };

            _context.Entradas.AddRange(entrada1, entrada2, entrada3);
            await _context.SaveChangesAsync();

            return new List<Entrada> { entrada1, entrada2, entrada3 };
        }

        private async Task<List<Pregunta>> CrearPreguntasAsync(List<Entrada> entradas, List<Miembro> miembros)
        {
            var preguntas = new List<Pregunta>
            {
                new() { Texto = "¿Sirve pausar el entrenamiento unos días y volver con RPE bajo?", Fecha = DateTime.Now, Entrada = entradas[0], Miembro = miembros[7], Activa = true },
                new() { Texto = "¿Conviene maxear el perk de percepción si voy sniper?", Fecha = DateTime.Now, Entrada = entradas[1], Miembro = miembros[5], Activa = true },
                new() { Texto = "¿El truco es el almidón de maíz o el aceite bien caliente?", Fecha = DateTime.Now, Entrada = entradas[2], Miembro = miembros[3], Activa = true }
            };

            _context.Preguntas.AddRange(preguntas);
            await _context.SaveChangesAsync();

            return preguntas;
        }

        private async Task<List<Respuesta>> CrearRespuestasAsync(List<Pregunta> preguntas, List<Miembro> miembros)
        {
            var respuestas = new List<Respuesta>
            {
                new() { Texto = "Sí, hacer un mini reset de carga con RPE 6 durante una semana me ayudó mucho.", Fecha = DateTime.Now, Pregunta = preguntas[0], Miembro = miembros[9] },
                new() { Texto = "Percepción es clave si jugás en sigilo, pero también VATS y Agilidad te suman mucho.", Fecha = DateTime.Now, Pregunta = preguntas[1], Miembro = miembros[6] },
                new() { Texto = "Secalo con papel antes de cocinar y usá maicena + sartén bien caliente. Sale crocante seguro.", Fecha = DateTime.Now, Pregunta = preguntas[2], Miembro = miembros[8] }
            };

            _context.Respuestas.AddRange(respuestas);
            await _context.SaveChangesAsync();

            return respuestas;
        }

        private async Task CrearReaccionesAsync(List<Respuesta> respuestas, List<Miembro> miembros)
        {
            var reacciones = new List<Reaccion>
            {
                new() { Texto = "Muy buen consejo, gracias.", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembros[0], Respuesta = respuestas[0] },
                new() { Texto = "No sabía lo de la percepción, gracias!", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembros[1], Respuesta = respuestas[1] }
            };

            _context.Reacciones.AddRange(reacciones);
            await _context.SaveChangesAsync();
        }

        private async Task CrearHabilitacionesAsync(List<Entrada> entradas, List<Miembro> miembros)
        {
            var habilitacion = new Habilitacion
            {
                Entrada = entradas[1],
                Miembro = miembros[8]
            };

            _context.Habilitaciones.Add(habilitacion);
            await _context.SaveChangesAsync();
        }
    }
}

   