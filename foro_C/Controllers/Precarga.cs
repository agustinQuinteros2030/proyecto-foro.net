using foro_C.Data;
using foro_C.Helpers;
using foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            // 3 miembros principales: 0 (iron.agus), 1 (gamer.ari), 2 (chef.lu)
            var entradas = new List<Entrada>
    {
        // Powerlifting (4 entradas)
        new() { Titulo = "¿Cómo mejorar el press de banca?", Texto = "Estoy estancado en mi 1RM de banca hace 3 meses. ¿Qué me recomiendan?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[0], Miembro = miembros[0] },
        new() { Titulo = "Errores comunes en peso muerto", Texto = "¿Qué errores debo evitar al hacer peso muerto?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[0], Miembro = miembros[0] },
        new() { Titulo = "¿Cómo progresar en sentadillas?", Texto = "No logro subir de peso en sentadillas, ¿algún consejo?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[0], Miembro = miembros[0] },
        new() { Titulo = "¿Cómo evitar lesiones en press militar?", Texto = "Me duele el hombro al hacerlo, ¿qué hago mal?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[0], Miembro = miembros[0] },

        // Videojuegos (3 entradas)
        new() { Titulo = "Fallout New Vegas: ¿mejor build de sniper?", Texto = "Estoy volviendo al juego y quiero probar algo con VATS y sigilo.", Fecha = DateTime.Now, Privada = true, Categoria = categorias[1], Miembro = miembros[1] },
        new() { Titulo = "¿Qué monitor gamer recomiendan?", Texto = "Busco uno de 144Hz para FPS, ¿alguna marca?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[1], Miembro = miembros[1] },
        new() { Titulo = "¿Vale la pena la PS5?", Texto = "¿O espero a la próxima generación?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[1], Miembro = miembros[1] },

        // Cocina (3 entradas)
        new() { Titulo = "¿Cómo hago tofu crocante como en los restaurantes?", Texto = "Intenté mil veces pero siempre me queda blando o gomoso.", Fecha = DateTime.Now, Privada = false, Categoria = categorias[2], Miembro = miembros[2] },
        new() { Titulo = "Receta de pan sin gluten", Texto = "¿Alguien tiene una receta fácil y rica?", Fecha = DateTime.Now, Privada = false, Categoria = categorias[2], Miembro = miembros[2] },
        new() { Titulo = "¿Cómo hacer sushi en casa?", Texto = "Tips para principiantes y errores a evitar.", Fecha = DateTime.Now, Privada = false, Categoria = categorias[2], Miembro = miembros[2] }
    };

            _context.Entradas.AddRange(entradas);
            await _context.SaveChangesAsync();
            return entradas;
        }

        private async Task<List<Pregunta>> CrearPreguntasAsync(List<Entrada> entradas, List<Miembro> miembros)
        {
            var preguntas = new List<Pregunta>
    {
        // Powerlifting (varias preguntas en las primeras 2 entradas)
        new() { Texto = "¿Sirve pausar el entrenamiento unos días y volver con RPE bajo?", Fecha = DateTime.Now, Entrada = entradas[0], Miembro = miembros[7], Activa = true },
        new() { Texto = "¿Qué opinan de los accesorios para banca?", Fecha = DateTime.Now, Entrada = entradas[0], Miembro = miembros[3], Activa = true },
        new() { Texto = "¿Cuántas repeticiones recomiendan para fuerza?", Fecha = DateTime.Now, Entrada = entradas[1], Miembro = miembros[2], Activa = true },
        new() { Texto = "¿Cómo evitar redondear la espalda?", Fecha = DateTime.Now, Entrada = entradas[1], Miembro = miembros[7], Activa = true },
        // Videojuegos (varias preguntas en la primera entrada)
        new() { Texto = "¿Conviene maxear el perk de percepción si voy sniper?", Fecha = DateTime.Now, Entrada = entradas[4], Miembro = miembros[5], Activa = true },
        new() { Texto = "¿Qué armas tienen mejor sinergia con sigilo?", Fecha = DateTime.Now, Entrada = entradas[4], Miembro = miembros[6], Activa = true },
        // Cocina (varias preguntas en la primera entrada)
        new() { Texto = "¿El truco es el almidón de maíz o el aceite bien caliente?", Fecha = DateTime.Now, Entrada = entradas[7], Miembro = miembros[3], Activa = true },
        new() { Texto = "¿Qué tofu recomiendan comprar?", Fecha = DateTime.Now, Entrada = entradas[7], Miembro = miembros[4], Activa = true },
        // Una pregunta para cada entrada restante
        new() { Texto = "¿Qué hacer si me duele el hombro al hacer press militar?", Fecha = DateTime.Now, Entrada = entradas[3], Miembro = miembros[1], Activa = true },
        new() { Texto = "¿IPS o TN para gaming?", Fecha = DateTime.Now, Entrada = entradas[5], Miembro = miembros[5], Activa = true },
        new() { Texto = "¿La PS5 tiene retrocompatibilidad?", Fecha = DateTime.Now, Entrada = entradas[6], Miembro = miembros[8], Activa = true },
        new() { Texto = "¿Qué harinas sin gluten funcionan mejor?", Fecha = DateTime.Now, Entrada = entradas[8], Miembro = miembros[6], Activa = true },
        new() { Texto = "¿Qué arroz es mejor para sushi?", Fecha = DateTime.Now, Entrada = entradas[9], Miembro = miembros[9], Activa = true }
    };

            _context.Preguntas.AddRange(preguntas);
            await _context.SaveChangesAsync();
            return preguntas;
        }

        private async Task<List<Respuesta>> CrearRespuestasAsync(List<Pregunta> preguntas, List<Miembro> miembros)
        {
            var respuestas = new List<Respuesta>
    {
        // Powerlifting
        new() { Texto = "Sí, hacer un mini reset de carga con RPE 6 durante una semana me ayudó mucho.", Fecha = DateTime.Now, Pregunta = preguntas[0], Miembro = miembros[9] },
        new() { Texto = "Los accesorios ayudan, pero no reemplazan la técnica.", Fecha = DateTime.Now, Pregunta = preguntas[1], Miembro = miembros[8] },
        new() { Texto = "Entre 3 y 5 repeticiones, 4 series.", Fecha = DateTime.Now, Pregunta = preguntas[2], Miembro = miembros[5] },
        new() { Texto = "Practica con poco peso y mucha técnica.", Fecha = DateTime.Now, Pregunta = preguntas[3], Miembro = miembros[2] },
        // Videojuegos
        new() { Texto = "Percepción es clave si jugás en sigilo, pero también VATS y Agilidad te suman mucho.", Fecha = DateTime.Now, Pregunta = preguntas[4], Miembro = miembros[6] },
        new() { Texto = "El rifle silenciado es lo mejor para sigilo.", Fecha = DateTime.Now, Pregunta = preguntas[5], Miembro = miembros[5] },
        // Cocina
        new() { Texto = "Secalo con papel antes de cocinar y usá maicena + sartén bien caliente. Sale crocante seguro.", Fecha = DateTime.Now, Pregunta = preguntas[6], Miembro = miembros[7] },
        new() { Texto = "El tofu firme es el mejor para crocancia.", Fecha = DateTime.Now, Pregunta = preguntas[7], Miembro = miembros[6] },
        // Resto
        new() { Texto = "Rotaciones y bandas elásticas ayudan mucho.", Fecha = DateTime.Now, Pregunta = preguntas[8], Miembro = miembros[8] },
        new() { Texto = "IPS para colores, TN para velocidad.", Fecha = DateTime.Now, Pregunta = preguntas[9], Miembro = miembros[4] },
        new() { Texto = "Sí, pero no todos los juegos.", Fecha = DateTime.Now, Pregunta = preguntas[10], Miembro = miembros[1] },
        new() { Texto = "La mezcla de arroz y maíz funciona bien.", Fecha = DateTime.Now, Pregunta = preguntas[11], Miembro = miembros[3] },
        new() { Texto = "El arroz glutinoso es el ideal.", Fecha = DateTime.Now, Pregunta = preguntas[12], Miembro = miembros[0] }
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
        new() { Texto = "¡Me sirvió mucho!", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembros[1], Respuesta = respuestas[1] },
        new() { Texto = "No sabía lo de la percepción, gracias!", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembros[2], Respuesta = respuestas[4] },
        new() { Texto = "¡Gran dato!", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembros[3], Respuesta = respuestas[5] },
        new() { Texto = "¡Voy a probarlo!", Fecha = DateTime.Now, Tipo = TipoReaccion.MeGusta, Miembro = miembros[4], Respuesta = respuestas[6] }
    };

            _context.Reacciones.AddRange(reacciones);
            await _context.SaveChangesAsync();
        }

        private async Task CrearHabilitacionesAsync(List<Entrada> entradas, List<Miembro> miembros)
        {
            var habilitaciones = new List<Habilitacion>
    {
        // Ejemplo: habilitar a distintos miembros para distintas entradas
        new() { Entrada = entradas[1], Miembro = miembros[3] },
        new() { Entrada = entradas[4], Miembro = miembros[5] },
        new() { Entrada = entradas[7], Miembro = miembros[6] },
        new() { Entrada = entradas[0], Miembro = miembros[2] },
        new() { Entrada = entradas[2], Miembro = miembros[8] }
    };

            _context.Habilitaciones.AddRange(habilitaciones);
            await _context.SaveChangesAsync();
        }
    }
}