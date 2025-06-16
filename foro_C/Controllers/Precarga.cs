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

                // Crear Miembros
                var miembroAgus = new Miembro { UserName = "iron.agus", Nombre = "Agustín", Apellido = "Quinteros", Email = "agus@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 10101010 };
                var miembroAri = new Miembro { UserName = "gamer.ari", Nombre = "Ariel", Apellido = "Mendoza", Email = "ari@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 20202020 };
                var miembroLu = new Miembro { UserName = "chef.lu", Nombre = "Lucía", Apellido = "Paz", Email = "lucia@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 30303030 };
                var miembroRo = new Miembro { UserName = "dev.ro", Nombre = "Romina", Apellido = "Torres", Email = "romi@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 40404040 };
                var miembroCaro = new Miembro { UserName = "pwr.caro", Nombre = "Carolina", Apellido = "Benítez", Email = "caro@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 80808080 };
                var miembroManu = new Miembro { UserName = "anime.manu", Nombre = "Manuel", Apellido = "Saito", Email = "manu@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 60606060 };
                var miembroKate = new Miembro { UserName = "plant.kate", Nombre = "Katherine", Apellido = "Flores", Email = "kate@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 90909090 };
                var miembroIvan = new Miembro { UserName = "trip.ivan", Nombre = "Iván", Apellido = "Delgado", Email = "ivan@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 11112222 };
                var miembroEma = new Miembro { UserName = "musica.ema", Nombre = "Emanuel", Apellido = "Pérez", Email = "ema@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 70707070 };

                var miembros = new List<Miembro> {
                miembroAgus, miembroAri, miembroLu, miembroRo, miembroCaro,
                miembroManu, miembroKate, miembroIvan, miembroEma
            };

                foreach (var miembro in miembros)
                {
                    await userManager.CreateAsync(miembro, "Password1!");
                    await userManager.AddToRoleAsync(miembro, "Miembro");
                }

                // Categorías
                var categoriaPwr = new Categoria { Nombre = "Powerlifting" };
                var categoriaGames = new Categoria { Nombre = "Videojuegos" };
                var categoriaCocina = new Categoria { Nombre = "Cocina" };
                context.Categorias.AddRange(categoriaPwr, categoriaGames, categoriaCocina);
                await context.SaveChangesAsync();

                // Entradas
                var entrada1 = new Entrada
                {
                    Titulo = "¿Cómo mejorar el press de banca?",
                    Texto = "Estoy estancado en mi 1RM de banca hace 3 meses. ¿Qué me recomiendan?",
                    Fecha = DateTime.Now,
                    Privada = false,
                    Categoria = categoriaPwr,
                    Miembro = miembroAgus
                };
                var entrada2 = new Entrada
                {
                    Titulo = "Fallout New Vegas: ¿mejor build de sniper?",
                    Texto = "Estoy volviendo al juego y quiero probar algo con VATS y sigilo.",
                    Fecha = DateTime.Now,
                    Privada = true,
                    Categoria = categoriaGames,
                    Miembro = miembroAri
                };
                var entrada3 = new Entrada
                {
                    Titulo = "¿Cómo hago tofu crocante como en los restaurantes?",
                    Texto = "Intenté mil veces pero siempre me queda blando o gomoso.",
                    Fecha = DateTime.Now,
                    Privada = false,
                    Categoria = categoriaCocina,
                    Miembro = miembroLu
                };
                context.Entradas.AddRange(entrada1, entrada2, entrada3);
                await context.SaveChangesAsync();

                // Preguntas
                var pregunta1 = new Pregunta { Texto = "¿Sirve pausar el entrenamiento unos días y volver con RPE bajo?", Fecha = DateTime.Now, Entrada = entrada1, Miembro = miembroCaro, Activa = true };
                var pregunta2 = new Pregunta { Texto = "¿Conviene maxear el perk de percepción si voy sniper?", Fecha = DateTime.Now, Entrada = entrada2, Miembro = miembroManu, Activa = true };
                var pregunta3 = new Pregunta { Texto = "¿El truco es el almidón de maíz o el aceite bien caliente?", Fecha = DateTime.Now, Entrada = entrada3, Miembro = miembroRo, Activa = true };
                context.Preguntas.AddRange(pregunta1, pregunta2, pregunta3);
                await context.SaveChangesAsync();

                // Respuestas
                var respuesta1 = new Respuesta { Texto = "Sí, hacer un mini reset de carga con RPE 6 durante una semana me ayudó mucho.", Fecha = DateTime.Now, Miembro = miembroIvan, Pregunta = pregunta1 };
                var respuesta2 = new Respuesta { Texto = "Percepción es clave si jugás en sigilo, pero también VATS y Agilidad te suman mucho.", Fecha = DateTime.Now, Miembro = miembroEma, Pregunta = pregunta2 };
                var respuesta3 = new Respuesta { Texto = "Secalo con papel antes de cocinar y usá maicena + sartén bien caliente. Sale crocante seguro.", Fecha = DateTime.Now, Miembro = miembroKate, Pregunta = pregunta3 };
                context.Respuestas.AddRange(respuesta1, respuesta2, respuesta3);
                await context.SaveChangesAsync();

                // Reacciones
                var reaccion1 = new Reaccion { Texto = "Muy buen consejo, gracias.", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembroAgus, Respuesta = respuesta1 };
                var reaccion2 = new Reaccion { Texto = "No sabía lo de la percepción, gracias!", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembroAri, Respuesta = respuesta2 };
                context.Reacciones.AddRange(reaccion1, reaccion2);
                await context.SaveChangesAsync();

                // Habilitación
                var habilitacion = new Habilitacion { Entrada = entrada2, Miembro = miembroKate };
                context.Habilitaciones.Add(habilitacion);
                await context.SaveChangesAsync();
            }
        }

    }
}