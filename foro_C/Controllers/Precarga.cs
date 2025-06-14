using foro_C.Data;
using foro_C.Models;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foro_C.Controllers
{
    public class Precarga1 : Controller
    {

        public static async Task EnviarPrecargaAsync(
      ForoContext context,
      RoleManager<Rol> roleManager,
      UserManager<Persona> userManager)
        {
            {
                // Crear roles si no existen
                string[] roles = { "Administrador", "Miembro" };
                foreach (var rol in roles)
                {
                    if (!await roleManager.RoleExistsAsync(rol))
                    {
                        await roleManager.CreateAsync(new Rol(rol));
                    }
                }

                // Crear Empleados
                var empleados = new List<Empleado>
            {
                new() { UserName = "c.gimenez", Nombre = "Carlos", Apellido = "Giménez", Email = "carlos@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 11111111, Legajo = "E001A" },
                new() { UserName = "l.ramos", Nombre = "Laura", Apellido = "Ramos", Email = "laura@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 22222222, Legajo = "E002B" },
                new() { UserName = "t.lopez", Nombre = "Tomás", Apellido = "López", Email = "tomas@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 33333333, Legajo = "E003C" }
            };

                foreach (var emp in empleados)
                {
                    await userManager.CreateAsync(emp, "Password1!");
                    await userManager.AddToRoleAsync(emp, "Administrador");
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