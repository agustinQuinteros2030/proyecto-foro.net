using foro_C.Data;
using foro_C.Helpers;
using foro_C.Models;
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
        private readonly UserManager<Persona> _usermanager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContext _context;
        private readonly ILogger<Precarga1> _logger;

        private readonly List<string> _roles = new()
        {
            Confings.AdminRole,
            Confings.MiembroRole

        };
        public Precarga1(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ForoContext context, ILogger<Precarga1> logger)
        {

            _usermanager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;

        }
        // Action method for the route
        public async Task<IActionResult> Index()
        {
            await InicializarDatosAsync();
            return RedirectToAction("Home", "Index");
        }

        public async Task InicializarDatosAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando carga de datos iniciales...");

                if (await _context.Roles.AnyAsync())
                {
                    _logger.LogInformation("Los datos ya están cargados. Saltando precarga.");
                    return;
                }

                await CrearRoles();
                await CrearAdmin();
                await CrearMiembros();
                await CrearCategorias();
                await CrearEntradas();
                await CrearPreguntas();
                await CrearRespuestas();
                await CrearReacciones();

                _logger.LogInformation("Carga de datos iniciales completada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la carga de datos iniciales");
                throw;
            }
        }




        private async Task CrearRoles()
        {

            foreach (var rol in _roles)
            {
                if (!await _roleManager.RoleExistsAsync(rol))
                {
                    await _roleManager.CreateAsync(new Rol(rol));
                }
            }

        }

        private async Task CrearAdmin()
        {
            // First Admin User
            var admin1User = await _usermanager.FindByNameAsync("empleado1");
            if (admin1User == null)
            {
                var empleado1 = new Empleado
                {
                    Legajo = "01EP01",
                    UserName = "empleado1",
                    Nombre = "Empleado",
                    Apellido = "Uno",
                    Email = "empleado1@ort.edu.ar",
                    FechaAlta = DateTime.Now,
                    Telefono = 12345679
                };
                var result = await _usermanager.CreateAsync(empleado1, Confings.PasswordGenerica);
                if (result.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(empleado1, Confings.AdminRole);
                }
            }

            // Second Admin User
            var admin2User = await _usermanager.FindByNameAsync("empleadorrhh1");
            if (admin2User == null)
            {
                var empleadorrhh1 = new Empleado
                {
                    Legajo = "01EP02",
                    UserName = "empleadorrhh1",
                    Nombre = "Empleado",
                    Apellido = "RRHH",
                    Email = "empleadorrhh1@ort.edu.ar",
                    FechaAlta = DateTime.Now,
                    Telefono = 12345680
                };
                var result = await _usermanager.CreateAsync(empleadorrhh1, Confings.PasswordGenerica);
                if (result.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(empleadorrhh1, Confings.AdminRole);
                }
            }

            var admin3User = await _usermanager.FindByNameAsync("empleado2");
            if (admin3User == null)
            {
                var empleado3 = new Empleado
                {
                    Legajo = "01EP03",
                    UserName = "empleado2",
                    Nombre = "Empleado",
                    Apellido = "Uno",
                    Email = "empleado2@ort.edu.ar",
                    FechaAlta = DateTime.Now,
                    Telefono = 12345679
                };
                var result = await _usermanager.CreateAsync(empleado3, Confings.PasswordGenerica);
                if (result.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(empleado3, Confings.AdminRole);
                }
            }


        }


        private async Task CrearMiembros()
        {
            if (!_context.Miembros.Any())
            {
                var miembros = new List<Miembro>
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
                foreach (var miembro in miembros)
                {
                    var result = await _usermanager.CreateAsync(miembro, Confings.PasswordGenerica);
                    if (result.Succeeded)
                    {
                        await _usermanager.AddToRoleAsync(miembro, Confings.MiembroRole);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearCategorias()
        {
            if (!_context.Categorias.Any())
            {
                var categorias = new List<Categoria>
                {
                    new() { Nombre = "Powerlifting" },
                    new() { Nombre = "Videojuegos" },
                    new() { Nombre = "Cocina" },
                    new() { Nombre = "Programación" },
                    new() { Nombre = "Viajes" },
                    new() { Nombre = "Música" },
                    new() { Nombre = "Anime" },
                    new() { Nombre = "Jardinería" }
                };
                _context.Categorias.AddRange(categorias);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearEntradas()
        {
            if (!_context.Entradas.Any())
            {
                var categorias = _context.Categorias.ToList();
                var miembros = _context.Miembros.ToList();

                var entrada1 = new Entrada
                {
                    Titulo = "¿Cómo mejorar el press de banca?",
                    Texto = "Estoy estancado en mi 1RM de banca hace 3 meses. ¿Qué me recomiendan?",
                    Fecha = DateTime.Now,
                    Privada = false,
                    Categoria = categorias.First(c => c.Nombre == "Powerlifting"),
                    Miembro = miembros.First(m => m.UserName == "iron.agus")
                };

                var entrada2 = new Entrada
                {
                    Titulo = "Fallout New Vegas: ¿mejor build de sniper?",
                    Texto = "Estoy volviendo al juego y quiero probar algo con VATS y sigilo.",
                    Fecha = DateTime.Now,
                    Privada = true,
                    Categoria = categorias.First(c => c.Nombre == "Videojuegos"),
                    Miembro = miembros.First(m => m.UserName == "gamer.ari")
                };

                var entrada3 = new Entrada
                {
                    Titulo = "¿Cómo hago tofu crocante como en los restaurantes?",
                    Texto = "Intenté mil veces pero siempre me queda blando o gomoso.",
                    Fecha = DateTime.Now,
                    Privada = false,
                    Categoria = categorias.First(c => c.Nombre == "Cocina"),
                    Miembro = miembros.First(m => m.UserName == "chef.lu")
                };

                _context.Entradas.AddRange(entrada1, entrada2, entrada3);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearPreguntas()
        {
            if (!_context.Preguntas.Any())
            {
                var entradas = _context.Entradas.ToList();
                var miembros = _context.Miembros.ToList();

                var pregunta1 = new Pregunta
                {
                    Texto = "¿Sirve pausar el entrenamiento unos días y volver con RPE bajo?",
                    Fecha = DateTime.Now,
                    Entrada = entradas.First(e => e.Titulo.Contains("press de banca")),
                    Miembro = miembros.First(m => m.UserName == "pwr.caro"),
                    Activa = true
                };

                var pregunta2 = new Pregunta
                {
                    Texto = "¿Conviene maxear el perk de percepción si voy sniper?",
                    Fecha = DateTime.Now,
                    Entrada = entradas.First(e => e.Titulo.Contains("Fallout New Vegas")),
                    Miembro = miembros.First(m => m.UserName == "anime.manu"),
                    Activa = true
                };

                var pregunta3 = new Pregunta
                {
                    Texto = "¿El truco es el almidón de maíz o el aceite bien caliente?",
                    Fecha = DateTime.Now,
                    Entrada = entradas.First(e => e.Titulo.Contains("tofu crocante")),
                    Miembro = miembros.First(m => m.UserName == "dev.ro"),
                    Activa = true
                };

                _context.Preguntas.AddRange(pregunta1, pregunta2, pregunta3);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearRespuestas()
        {
            if (!_context.Respuestas.Any())
            {
                var preguntas = _context.Preguntas.ToList();
                var miembros = _context.Miembros.ToList();

                var respuesta1 = new Respuesta
                {
                    Texto = "Sí, hacer un mini reset de carga con RPE 6 durante una semana me ayudó mucho.",
                    Fecha = DateTime.Now,
                    Miembro = miembros.First(m => m.UserName == "trip.ivan"),
                    Pregunta = preguntas.First(p => p.Texto.Contains("pausar el entrenamiento"))
                };

                var respuesta2 = new Respuesta
                {
                    Texto = "Percepción es clave si jugás en sigilo, pero también VATS y Agilidad te suman mucho.",
                    Fecha = DateTime.Now,
                    Miembro = miembros.First(m => m.UserName == "musica.ema"),
                    Pregunta = preguntas.First(p => p.Texto.Contains("perk de percepción"))
                };

                var respuesta3 = new Respuesta
                {
                    Texto = "Secalo con papel antes de cocinar y usá maicena + sarten bien caliente. Sale crocante seguro.",
                    Fecha = DateTime.Now,
                    Miembro = miembros.First(m => m.UserName == "plant.kate"),
                    Pregunta = preguntas.First(p => p.Texto.Contains("almidón de maíz"))
                };

                _context.Respuestas.AddRange(respuesta1, respuesta2, respuesta3);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearReacciones()
        {
            if (!_context.Reacciones.Any())
            {
                var respuestas = _context.Respuestas.ToList();
                var miembros = _context.Miembros.ToList();

                var reaccion1 = new Reaccion
                {
                    Texto = "Muy buen consejo, gracias.",
                    Fecha = DateTime.Now,
                    Tipo = TipoReaccion.MeGusta,
                    Miembro = miembros.First(m => m.UserName == "iron.agus"),
                    Respuesta = respuestas.First(r => r.Texto.Contains("mini reset de carga"))
                };

                var reaccion2 = new Reaccion
                {
                    Texto = "No sabía lo de la percepción, gracias!",
                    Fecha = DateTime.Now,
                    Tipo = TipoReaccion.MeGusta,
                    Miembro = miembros.First(m => m.UserName == "gamer.ari"),
                    Respuesta = respuestas.First(r => r.Texto.Contains("Percepción es clave"))
                };

                _context.Reacciones.AddRange(reaccion1, reaccion2);
                await _context.SaveChangesAsync();
            }
        }
    }
}