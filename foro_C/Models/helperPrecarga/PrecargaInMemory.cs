using foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace foro_C.Data
{
    public static class Precarga
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

                    // Crear miembro extra
                    var miembroZoe = new Miembro { UserName = "tech.zoe", Nombre = "Zoe", Apellido = "Martínez", Email = "zoe@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 50505050 };
                    await userManager.CreateAsync(miembroZoe, "Password1!");
                    await userManager.AddToRoleAsync(miembroZoe, "Miembro");

                    // Categorías
                    var categoriaPwr = new Categoria { Nombre = "Powerlifting" };
                    var categoriaGames = new Categoria { Nombre = "Videojuegos" };
                    var categoriaCocina = new Categoria { Nombre = "Cocina" };
                    var categoriaMusica = new Categoria { Nombre = "Música" };
                    var categoriaAnime = new Categoria { Nombre = "Anime" };
                    context.Categorias.AddRange(categoriaPwr, categoriaGames, categoriaCocina, categoriaMusica, categoriaAnime);
                    await context.SaveChangesAsync();

                    // Entradas (13 en total)
                    var entradas = new List<Entrada>
                {
                    new Entrada { Titulo = "¿Cómo mejorar el press de banca?", Texto = "Estoy estancado en mi 1RM de banca hace 3 meses. ¿Qué me recomiendan?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaPwr, Miembro = miembroAgus },
                    new Entrada { Titulo = "Fallout New Vegas: ¿mejor build de sniper?", Texto = "Estoy volviendo al juego y quiero probar algo con VATS y sigilo.", Fecha = DateTime.Now, Privada = true, Categoria = categoriaGames, Miembro = miembroAri },
                    new Entrada { Titulo = "¿Cómo hago tofu crocante como en los restaurantes?", Texto = "Intenté mil veces pero siempre me queda blando o gomoso.", Fecha = DateTime.Now, Privada = false, Categoria = categoriaCocina, Miembro = miembroLu },
                    new Entrada { Titulo = "¿Qué bandas argentinas indie recomiendan?", Texto = "Busco algo tranqui para estudiar, tipo El Mató o Bandalos Chinos.", Fecha = DateTime.Now, Privada = false, Categoria = categoriaMusica, Miembro = miembroEma },
                    new Entrada { Titulo = "¿Qué anime te voló la cabeza últimamente?", Texto = "Vi Vinland Saga y me encantó, quiero más cosas así.", Fecha = DateTime.Now, Privada = false, Categoria = categoriaAnime, Miembro = miembroManu },
                    new Entrada { Titulo = "¿Qué opinan del uso de cinturón en sentadillas?", Texto = "Algunos me dicen que no lo use hasta levantar pesado, otros que lo entrene desde antes. ¿Qué hacen ustedes?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaPwr, Miembro = miembroAgus },
                    new Entrada { Titulo = "Mejor setup para el press banca según su experiencia", Texto = "Quiero optimizar el arco, la retracción escapular, todo. ¿Consejos?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaPwr, Miembro = miembroAgus },
                    new Entrada { Titulo = "¿Cómo se lidia con el miedo a fallar en competencia?", Texto = "Siento presión antes del 3er intento. ¿A alguno más le pasa?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaPwr, Miembro = miembroAgus },
                    new Entrada { Titulo = "¿Alguien juega juegos de gestión tipo RimWorld o Factorio?", Texto = "Estoy re viciado con estos juegos pero no tengo con quién hablar de ellos jaja.", Fecha = DateTime.Now, Privada = false, Categoria = categoriaGames, Miembro = miembroAgus },
                    new Entrada { Titulo = "Tofu ahumado: ¿vale la pena comprarlo ya hecho?", Texto = "Probé hacerlo casero pero me quedó raro. ¿Recomiendan marcas?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaCocina, Miembro = miembroAgus },
                    new Entrada { Titulo = "¿Qué especias usás sí o sí en tus comidas?", Texto = "Me gusta experimentar pero a veces no sé por dónde empezar. ¿Ideas?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaCocina, Miembro = miembroLu },
                    new Entrada { Titulo = "Recetas veganas rápidas para después del entrenamiento", Texto = "No tengo ganas de cocinar después del gym, pero quiero seguir comiendo sano.", Fecha = DateTime.Now, Privada = false, Categoria = categoriaCocina, Miembro = miembroLu },
                    new Entrada { Titulo = "¿Qué framework usan para hacer APIs REST en .NET?", Texto = "Estoy arrancando y no sé si ir con Minimal API, MVC o algo más. ¿Qué recomiendan para un proyecto serio?", Fecha = DateTime.Now, Privada = false, Categoria = categoriaGames, Miembro = miembroZoe }
                };
                    context.Entradas.AddRange(entradas);
                    await context.SaveChangesAsync();

                    // Aquí continuarías con preguntas, respuestas, reacciones y habilitaciones según las nuevas entradas...
                }
            }
        }
    }


    


    