using foro_C.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace foro_C.Models.helperPrecarga
{
    public static class Precarga
    {

        public static void EnviarPrecarga(ForoContext context)
        {
            
            if (!context.Empleados.Any())
            {
                var empleados = new List<Empleado>
                {
                    new() { UserName = "c.gimenez", Nombre = "Carlos", Apellido = "Giménez", Email = "carlos@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 11111111, Legajo = "E001A" },
                    new() { UserName = "l.ramos", Nombre = "Laura", Apellido = "Ramos", Email = "laura@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 22222222, Legajo = "E002B" },
                    new() { UserName = "t.lopez", Nombre = "Tomás", Apellido = "López", Email = "tomas@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 33333333, Legajo = "E003C" }
                };
                context.Empleados.AddRange(empleados);
                context.SaveChanges();
            }

            // MIEMBROS
            if (!context.Miembros.Any())
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
                context.Miembros.AddRange(miembros);
                context.SaveChanges();

            }

            // CATEGORÍAS
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
            context.Categorias.AddRange(categorias);
            context.SaveChanges();


            //var miembro1id = context.Miembros.FirstOrDefault(m => m.UserName == "iron.agus");
            // ENTRADAS CON PREGUNTAS Y RESPUESTAS VARIADAS
            var entrada1 = new Entrada
            {
                Titulo = "¿Cómo mejorar el press de banca?",
                Texto = "Estoy estancado en mi 1RM de banca hace 3 meses. ¿Qué me recomiendan?",
                Fecha = DateTime.Now,
                Privada = false,
                Categoria = categorias.First(c => c.Nombre == "Powerlifting"),
                Miembro = context.Miembros.FirstOrDefault(m => m.UserName == "iron.agus")
            };

            var entrada2 = new Entrada
            {
                Titulo = "Fallout New Vegas: ¿mejor build de sniper?",
                Texto = "Estoy volviendo al juego y quiero probar algo con VATS y sigilo.",
                Fecha = DateTime.Now,
                Privada = true,
                Categoria = categorias.First(c => c.Nombre == "Videojuegos"),
                Miembro = context.Miembros.First(m => m.UserName == "gamer.ari")
                
            };

            var entrada3 = new Entrada
            {
                Titulo = "¿Cómo hago tofu crocante como en los restaurantes?",
                Texto = "Intenté mil veces pero siempre me queda blando o gomoso.",
                Fecha = DateTime.Now,
                Privada = false,
                Categoria = categorias.First(c => c.Nombre == "Cocina"),
                Miembro = context.Miembros.First(m => m.UserName == "chef.lu")
                
            };

            context.Entradas.AddRange(entrada1, entrada2, entrada3);
          

            // PREGUNTAS
            var pregunta1 = new Pregunta
            {
                Texto = "¿Sirve pausar el entrenamiento unos días y volver con RPE bajo?",
                Fecha = DateTime.Now,
                Entrada = entrada1,
                Miembro = context.Miembros.First(m => m.UserName == "pwr.caro"),
                Activa = true
            };

            var pregunta2 = new Pregunta
            {
                Texto = "¿Conviene maxear el perk de percepción si voy sniper?",
                Fecha = DateTime.Now,
                Entrada = entrada2,
                Miembro = context.Miembros.First(m => m.UserName == "anime.manu"),
                Activa = true
            };

            var pregunta3 = new Pregunta
            {
                Texto = "¿El truco es el almidón de maíz o el aceite bien caliente?",
                Fecha = DateTime.Now,
                Entrada = entrada3,
                Miembro = context.Miembros.First(m => m.UserName == "dev.ro"),
                Activa = true
            };

            context.Preguntas.AddRange(pregunta1, pregunta2, pregunta3);
          

            // RESPUESTAS
            var respuesta1 = new Respuesta
            {
                Texto = "Sí, hacer un mini reset de carga con RPE 6 durante una semana me ayudó mucho.",
                Fecha = DateTime.Now,
                Miembro = context.Miembros.First(m => m.UserName == "trip.ivan"),
                Pregunta = pregunta1
            };

            var respuesta2 = new Respuesta
            {
                Texto = "Percepción es clave si jugás en sigilo, pero también VATS y Agilidad te suman mucho.",
                Fecha = DateTime.Now,
                Miembro = context.Miembros.First(m => m.UserName == "musica.ema"),
                Pregunta = pregunta2
            };

            var respuesta3 = new Respuesta
            {
                Texto = "Secalo con papel antes de cocinar y usá maicena + sarten bien caliente. Sale crocante seguro.",
                Fecha = DateTime.Now,
                Miembro = context.Miembros.First(m => m.UserName == "plant.kate"),
                Pregunta = pregunta3
            };

            context.Respuestas.AddRange(respuesta1, respuesta2, respuesta3);
            

            // REACCIONES
            var reaccion1 = new Reaccion
            {
                Texto = "Muy buen consejo, gracias.",
                Fecha = DateTime.Now,
                Tipo = TipoReaccion.MeGusta,
                Miembro = context.Miembros.First(m => m.UserName == "iron.agus"),
                Respuesta = respuesta1
            };

            var reaccion2 = new Reaccion
            {
                Texto = "No sabía lo de la percepción, gracias!",
                Fecha = DateTime.Now,
                Tipo = TipoReaccion.MeGusta,
                Miembro = context.Miembros.First(m => m.UserName == "gamer.ari"),
                Respuesta = respuesta2
            };

            context.Reacciones.AddRange(reaccion1, reaccion2);

            // HABILITACIÓN para la entrada privada
            var habilitacion = new Habilitacion
            {
                Entrada = entrada2,
                Miembro = context.Miembros.First(m => m.UserName == "plant.kate")
            };
            context.Habilitaciones.Add(habilitacion);

            context.SaveChanges();
        }
    }
}
            
        





    

