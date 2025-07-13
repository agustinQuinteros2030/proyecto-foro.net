using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Foro2._0.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Foro2._0.Data
{


public static class Precarga
    {
        public static async Task Seed(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ForoContext context)
        {
            // Crear roles
            var roles = new[] { "ADMINISTRADOR", "MIEMBRO" };
            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new Rol { Name = rol, NormalizedName = rol.ToUpper() });
            }

            // Crear empleados
            var admin = new Empleado
            {
                UserName = "AgustinAdmin",
                Email = "agustinquinteros84@gmail.com",
                Nombre = "Marta",
                Apellido = "Quiroga",
                Telefono = "1122334455",
               
                FechaAlta = DateTime.Now,
                Legajo = "ADM001"
            };
            await userManager.CreateAsync(admin, "Password1!");
            await userManager.AddToRoleAsync(admin, "ADMINISTRADOR");

            var emp1 = new Empleado
            {
                UserName = "nicolasHR",
                Email = "nicolas@foro.com",
                Nombre = "Nicolás",
                Apellido = "Roldán",
                Telefono = "1198765432",
               
                FechaAlta = DateTime.Now,
                Legajo = "EMP002"
            };
            await userManager.CreateAsync(emp1, "Password1!");
            await userManager.AddToRoleAsync(emp1, "ADMINISTRADOR");

            var emp2 = new Empleado
            {
                UserName = "carlaSupport",
                Email = "carla@foro.com",
                Nombre = "Carla",
                Apellido = "Paz",
                Telefono = "1133445566",
              
                FechaAlta = DateTime.Now,
                Legajo = "EMP003"
            };
            await userManager.CreateAsync(emp2, "Password1!");
            await userManager.AddToRoleAsync(emp2, "ADMINISTRADOR");

            // Crear miembros
            var juan = new Miembro
            {
                UserName = "juanEstudiante",
                Email = "juan@foro.com",
                Nombre = "Juan",
                Apellido = "Martínez",
                Telefono = "116543210",
                
                FechaAlta = DateTime.Now
            };
            await userManager.CreateAsync(juan, "Password1!");
            await userManager.AddToRoleAsync(juan, "MIEMBRO");

            var lucia = new Miembro
            {
                UserName = "luciaFront",
                Email = "lucia@foro.com",
                Nombre = "Lucía",
                Apellido = "Giménez",
                Telefono = "115678901",
               
                FechaAlta = DateTime.Now
            };
            await userManager.CreateAsync(lucia, "Password1!");
            await userManager.AddToRoleAsync(lucia, "MIEMBRO");

            var agustin = new Miembro
            {
                UserName = "agusCode",
                Email = "agustin@foro.com",
                Nombre = "Agustín",
                Apellido = "Quinteros",
                Telefono = "114561239",
              
                FechaAlta = DateTime.Now
            };
            await userManager.CreateAsync(agustin, "Password1!");
            await userManager.AddToRoleAsync(agustin, "MIEMBRO");

            var franco = new Miembro
            {
                UserName = "francoDB",
                Email = "franco@foro.com",
                Nombre = "Franco",
                Apellido = "Ibarra",
                Telefono= "113212345",
                
                FechaAlta = DateTime.Now
            };
            await userManager.CreateAsync(franco, "Password1!");
            await userManager.AddToRoleAsync(franco, "MIEMBRO");

            var sofia = new Miembro
            {
                UserName = "sofiaUX",
                Email = "sofia@foro.com",
                Nombre = "Sofía",
                Apellido = "Vera",
                Telefono = "117654321",
               
                FechaAlta = DateTime.Now
            };
            await userManager.CreateAsync(sofia, "Password1!");
            await userManager.AddToRoleAsync(sofia, "MIEMBRO");

            // Guardar y obtener IDs
            await context.SaveChangesAsync();
            var miembros = await context.Miembros.ToListAsync();

            // Crear categorías
            var categorias = new List<Categoria>
        {
            new Categoria { Nombre = "ASP.NET Core" },
            new Categoria { Nombre = "Entity Framework" },
            new Categoria { Nombre = "Base de Datos" },
            new Categoria { Nombre = "JavaScript" },
            new Categoria { Nombre = "Frontend" },
            new Categoria { Nombre = "Redes" },
            new Categoria { Nombre = "Seguridad Informática" }
        };
            context.Categorias.AddRange(categorias);
            await context.SaveChangesAsync();

            #region Entradas

            var entrada1 = new Entrada
            {
                Titulo = "Problemas con Identity",
                Texto = "Estoy teniendo problemas para configurar Identity en mi proyecto y no entiendo bien cómo manejar los roles personalizados.",
                FechaCreacion = DateTime.Now.AddDays(-9),
                Privada = false,
                Miembro = juan,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "ASP.NET Core")
            };

            var entrada2 = new Entrada
            {
                Titulo = "Optimización de consultas en EF Core",
                Texto = "Tengo una consulta con varios JOINs que está muy lenta. ¿Qué estrategias de optimización recomiendan?",
                FechaCreacion = DateTime.Now.AddDays(-8),
                Privada = false,
                Miembro = franco,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "Entity Framework")
            };

            var entrada3 = new Entrada
            {
                Titulo = "Diseño de interfaces accesibles",
                Texto = "¿Qué prácticas y herramientas usan para garantizar accesibilidad en interfaces web?",
                FechaCreacion = DateTime.Now.AddDays(-7),
                Privada = true,
                Miembro = sofia,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "Frontend")
            };

            var entrada4 = new Entrada
            {
                Titulo = "Problemas al migrar una base de datos",
                Texto = "Estoy intentando migrar una base de datos desde SQLite a SQL Server y me da errores de tipo. ¿A alguien le pasó?",
                FechaCreacion = DateTime.Now.AddDays(-6),
                Privada = false,
                Miembro = agustin,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "Base de Datos")
            };

            var entrada5 = new Entrada
            {
                Titulo = "Dudas con validación de formularios JS",
                Texto = "Estoy tratando de hacer validaciones del lado del cliente con JavaScript puro. ¿Conviene usar alguna librería?",
                FechaCreacion = DateTime.Now.AddDays(-5),
                Privada = false,
                Miembro = lucia,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "JavaScript")
            };

            var entrada6 = new Entrada
            {
                Titulo = "Enrutamiento con ASP.NET Core",
                Texto = "Estoy confundido con cómo ASP.NET Core maneja los endpoints y rutas. ¿Alguien tiene una guía clara?",
                FechaCreacion = DateTime.Now.AddDays(-4),
                Privada = false,
                Miembro = juan,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "ASP.NET Core")
            };

            var entrada7 = new Entrada
            {
                Titulo = "VPN y seguridad de datos",
                Texto = "¿Qué tan seguro es usar VPN en redes públicas y qué protocolos recomiendan?",
                FechaCreacion = DateTime.Now.AddDays(-3),
                Privada = true,
                Miembro = franco,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "Seguridad Informática")
            };

            var entrada8 = new Entrada
            {
                Titulo = "Gestión de estado en Frontend",
                Texto = "¿Qué librerías o enfoques recomiendan para manejar estado en interfaces complejas?",
                FechaCreacion = DateTime.Now.AddDays(-2),
                Privada = false,
                Miembro = sofia,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "Frontend")
            };

            var entrada9 = new Entrada
            {
                Titulo = "Configurar puertos en routers domésticos",
                Texto = "Estoy intentando acceder a un servicio web local desde otra red y creo que es un tema de NAT. ¿Cómo lo configuro?",
                FechaCreacion = DateTime.Now.AddDays(-1),
                Privada = false,
                Miembro = agustin,
                Categoria = categorias.FirstOrDefault(c => c.Nombre == "Redes")
            };

            context.Entradas.AddRange(entrada1, entrada2, entrada3, entrada4, entrada5, entrada6, entrada7, entrada8, entrada9);
            await context.SaveChangesAsync();

            #endregion

            #region Preguntas

            var pregunta1 = new Pregunta
            {
                Texto = "¿Cómo se configura Identity para roles personalizados en ASP.NET Core?",
                Fecha = DateTime.Now.AddHours(-12),
                Entrada = entrada1,
                Miembro = agustin,
                Activa = true
            };

            var pregunta2 = new Pregunta
            {
                Texto = "¿Qué estrategias usan para optimizar JOINs complejos en EF Core?",
                Fecha = DateTime.Now.AddHours(-11),
                Entrada = entrada2,
                Miembro = juan,
                Activa = true
            };

            var pregunta3 = new Pregunta
            {
                Texto = "¿Qué herramientas recomiendan para validar accesibilidad en tiempo real?",
                Fecha = DateTime.Now.AddHours(-10),
                Entrada = entrada3,
                Miembro = lucia,
                Activa = true
            };

            var pregunta4 = new Pregunta
            {
                Texto = "¿Hay algo que deba considerar al migrar entre bases de datos distintas?",
                Fecha = DateTime.Now.AddHours(-9),
                Entrada = entrada4,
                Miembro = franco,
                Activa = true
            };

            var pregunta5 = new Pregunta
            {
                Texto = "¿Vale la pena usar librerías como Vuelidate o hacerlo a mano en JS?",
                Fecha = DateTime.Now.AddHours(-8),
                Entrada = entrada5,
                Miembro = sofia,
                Activa = true
            };

            var pregunta6 = new Pregunta
            {
                Texto = "¿Qué diferencias hay entre MapControllerRoute y MapDefaultControllerRoute?",
                Fecha = DateTime.Now.AddHours(-7),
                Entrada = entrada6,
                Miembro = lucia,
                Activa = true
            };

            var pregunta7 = new Pregunta
            {
                Texto = "¿OpenVPN o WireGuard para redes públicas?",
                Fecha = DateTime.Now.AddHours(-6),
                Entrada = entrada7,
                Miembro = agustin,
                Activa = true
            };

            var pregunta8 = new Pregunta
            {
                Texto = "¿Redux sigue siendo útil o hay alternativas más simples?",
                Fecha = DateTime.Now.AddHours(-5),
                Entrada = entrada8,
                Miembro = franco,
                Activa = true
            };

            var pregunta9 = new Pregunta
            {
                Texto = "¿Cómo configuro el reenvío de puertos en routers comunes?",
                Fecha = DateTime.Now.AddHours(-4),
                Entrada = entrada9,
                Miembro = juan,
                Activa = true
            };

            // 6 preguntas extra para otras entradas
            var pregunta10 = new Pregunta
            {
                Texto = "¿Qué errores comunes hay al usar claims personalizados?",
                Fecha = DateTime.Now.AddHours(-3),
                Entrada = entrada1,
                Miembro = sofia,
                Activa = true
            };

            var pregunta11 = new Pregunta
            {
                Texto = "¿Existen herramientas para analizar EF antes de lanzar a producción?",
                Fecha = DateTime.Now.AddHours(-2),
                Entrada = entrada2,
                Miembro = lucia,
                Activa = true
            };

            var pregunta12 = new Pregunta
            {
                Texto = "¿Hay alguna checklist para testing de accesibilidad web?",
                Fecha = DateTime.Now.AddHours(-1),
                Entrada = entrada3,
                Miembro = juan,
                Activa = true
            };

            var pregunta13 = new Pregunta
            {
                Texto = "¿Qué pasa si una migración falla a mitad de camino?",
                Fecha = DateTime.Now.AddMinutes(-45),
                Entrada = entrada4,
                Miembro = lucia,
                Activa = true
            };

            var pregunta14 = new Pregunta
            {
                Texto = "¿Conviene validar datos en frontend y backend o solo uno?",
                Fecha = DateTime.Now.AddMinutes(-30),
                Entrada = entrada5,
                Miembro = franco,
                Activa = true
            };

            var pregunta15 = new Pregunta
            {
                Texto = "¿Qué librerías de validación usan en React?",
                Fecha = DateTime.Now.AddMinutes(-15),
                Entrada = entrada8,
                Miembro = juan,
                Activa = true
            };

            context.Preguntas.AddRange(pregunta1, pregunta2, pregunta3, pregunta4, pregunta5, pregunta6, pregunta7, pregunta8, pregunta9,
                                       pregunta10, pregunta11, pregunta12, pregunta13, pregunta14, pregunta15);
            await context.SaveChangesAsync();

            #endregion



            #region Respuestas

            var respuesta1 = new Respuesta
            {
                Texto = "Podés usar `AddPolicy` con servicios de autorización en Program.cs para roles personalizados.",
                Fecha = DateTime.Now.AddMinutes(-50),
                Pregunta = pregunta1,
                Miembro = lucia
            };

            var respuesta2 = new Respuesta
            {
                Texto = "También podés usar claims personalizados y manejarlo desde el controlador con `[Authorize(Policy = \"AdminOnly\")]`.",
                Fecha = DateTime.Now.AddMinutes(-49),
                Pregunta = pregunta1,
                Miembro = sofia
            };

            var respuesta3 = new Respuesta
            {
                Texto = "Yo suelo crear índices compuestos y revisar los planes de ejecución con SQL Server Profiler.",
                Fecha = DateTime.Now.AddMinutes(-48),
                Pregunta = pregunta2,
                Miembro = agustin
            };

            var respuesta4 = new Respuesta
            {
                Texto = "Tené en cuenta evitar los `Include` innecesarios si no usás los datos luego.",
                Fecha = DateTime.Now.AddMinutes(-47),
                Pregunta = pregunta2,
                Miembro = sofia
            };

            var respuesta5 = new Respuesta
            {
                Texto = "Uso Axe para Chrome, es buenísima para detectar errores de contraste y navegación.",
                Fecha = DateTime.Now.AddMinutes(-46),
                Pregunta = pregunta3,
                Miembro = juan
            };

            var respuesta6 = new Respuesta
            {
                Texto = "Wave también es buena y tiene una versión online gratuita.",
                Fecha = DateTime.Now.AddMinutes(-45),
                Pregunta = pregunta3,
                Miembro = franco
            };

            var respuesta7 = new Respuesta
            {
                Texto = "Cuando migré de SQLite a SQL Server tuve problemas con las fechas. Tuve que mapearlas explícitamente.",
                Fecha = DateTime.Now.AddMinutes(-44),
                Pregunta = pregunta4,
                Miembro = lucia
            };

            var respuesta8 = new Respuesta
            {
                Texto = "Fijate también el tipo de claves primarias: SQL Server es más estricto que SQLite.",
                Fecha = DateTime.Now.AddMinutes(-43),
                Pregunta = pregunta4,
                Miembro = juan
            };

            var respuesta9 = new Respuesta
            {
                Texto = "Conviene usar librerías si el formulario es grande. Si es simple, con `addEventListener` alcanza.",
                Fecha = DateTime.Now.AddMinutes(-42),
                Pregunta = pregunta5,
                Miembro = agustin
            };

            var respuesta10 = new Respuesta
            {
                Texto = "Yo uso `yup` con React y me parece muy claro, incluso para errores personalizados.",
                Fecha = DateTime.Now.AddMinutes(-41),
                Pregunta = pregunta5,
                Miembro = franco
            };

            var respuesta11 = new Respuesta
            {
                Texto = "`MapControllerRoute` permite mayor control sobre las rutas definidas manualmente.",
                Fecha = DateTime.Now.AddMinutes(-40),
                Pregunta = pregunta6,
                Miembro = sofia
            };

            var respuesta12 = new Respuesta
            {
                Texto = "`MapDefaultControllerRoute` usa convenciones, es más simple pero menos flexible.",
                Fecha = DateTime.Now.AddMinutes(-39),
                Pregunta = pregunta6,
                Miembro = juan
            };

            var respuesta13 = new Respuesta
            {
                Texto = "WireGuard es más moderno y seguro, aunque OpenVPN tiene más compatibilidad.",
                Fecha = DateTime.Now.AddMinutes(-38),
                Pregunta = pregunta7,
                Miembro = lucia
            };

            var respuesta14 = new Respuesta
            {
                Texto = "Si tu router soporta ambos, probá WireGuard: consume menos batería y recursos.",
                Fecha = DateTime.Now.AddMinutes(-37),
                Pregunta = pregunta7,
                Miembro = sofia
            };

            var respuesta15 = new Respuesta
            {
                Texto = "Uso Zustand últimamente, es más liviano y fácil de mantener que Redux.",
                Fecha = DateTime.Now.AddMinutes(-36),
                Pregunta = pregunta8,
                Miembro = lucia
            };

            var respuesta16 = new Respuesta
            {
                Texto = "Si el proyecto es chico, context API es suficiente. No hace falta meter Redux.",
                Fecha = DateTime.Now.AddMinutes(-35),
                Pregunta = pregunta8,
                Miembro = agustin
            };

            var respuesta17 = new Respuesta
            {
                Texto = "Entrá al panel del router y buscá la sección NAT o Port Forwarding. Poné la IP del dispositivo local.",
                Fecha = DateTime.Now.AddMinutes(-34),
                Pregunta = pregunta9,
                Miembro = sofia
            };

            var respuesta18 = new Respuesta
            {
                Texto = "No te olvides de configurar el firewall de Windows para aceptar conexiones externas.",
                Fecha = DateTime.Now.AddMinutes(-33),
                Pregunta = pregunta9,
                Miembro = franco
            };

            var respuesta19 = new Respuesta
            {
                Texto = "Nunca usar claims sin validar su contenido. Un atacante puede spoofearlos.",
                Fecha = DateTime.Now.AddMinutes(-32),
                Pregunta = pregunta10,
                Miembro = juan
            };

            var respuesta20 = new Respuesta
            {
                Texto = "Podés usar Postman con JWT tokens falsos para testear qué tan bien validás.",
                Fecha = DateTime.Now.AddMinutes(-31),
                Pregunta = pregunta10,
                Miembro = franco
            };

            var respuesta21 = new Respuesta
            {
                Texto = "EF Power Tools es útil para analizar relaciones entre entidades visualmente.",
                Fecha = DateTime.Now.AddMinutes(-30),
                Pregunta = pregunta11,
                Miembro = sofia
            };

            var respuesta22 = new Respuesta
            {
                Texto = "También podés loguear las consultas generadas con `ToQueryString()` en los últimos EF.",
                Fecha = DateTime.Now.AddMinutes(-29),
                Pregunta = pregunta11,
                Miembro = agustin
            };

            var respuesta23 = new Respuesta
            {
                Texto = "Recomiendo usar la guía de WCAG como checklist base.",
                Fecha = DateTime.Now.AddMinutes(-28),
                Pregunta = pregunta12,
                Miembro = lucia
            };

            var respuesta24 = new Respuesta
            {
                Texto = "Podés usar la extensión Lighthouse de Chrome y sacar un informe completo.",
                Fecha = DateTime.Now.AddMinutes(-27),
                Pregunta = pregunta12,
                Miembro = franco
            };

            var respuesta25 = new Respuesta
            {
                Texto = "A mí me pasó y tuve que restaurar backup. Evitá hacer migraciones sin backup.",
                Fecha = DateTime.Now.AddMinutes(-26),
                Pregunta = pregunta13,
                Miembro = juan
            };

            context.Respuestas.AddRange(
                respuesta1, respuesta2, respuesta3, respuesta4, respuesta5,
                respuesta6, respuesta7, respuesta8, respuesta9, respuesta10,
                respuesta11, respuesta12, respuesta13, respuesta14, respuesta15,
                respuesta16, respuesta17, respuesta18, respuesta19, respuesta20,
                respuesta21, respuesta22, respuesta23, respuesta24, respuesta25
            );
            await context.SaveChangesAsync();

            #endregion
            #region Reacciones

            var reaccion1 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta1, Miembro = franco };
            var reaccion2 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta1, Miembro = juan };
            var reaccion3 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta2, Miembro = agustin };

            var reaccion4 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta3, Miembro = sofia };
            var reaccion5 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta3, Miembro = lucia };
            var reaccion6 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta4, Miembro = franco };

            var reaccion7 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta5, Miembro = agustin };
            var reaccion8 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta5, Miembro = sofia };
            var reaccion9 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta6, Miembro = lucia };
            var reaccion10 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta6, Miembro = agustin };

            var reaccion11 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta7, Miembro = franco };
            var reaccion12 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta8, Miembro = sofia };
            var reaccion13 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta9, Miembro = juan };
            var reaccion14 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta10, Miembro = franco };

            var reaccion15 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta11, Miembro = lucia };
            var reaccion16 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta12, Miembro = agustin };
            var reaccion17 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta13, Miembro = juan };
            var reaccion18 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta14, Miembro = franco };

            var reaccion19 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta15, Miembro = sofia };
            var reaccion20 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta16, Miembro = lucia };
            var reaccion21 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta17, Miembro = agustin };
            var reaccion22 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta18, Miembro = sofia };
            var reaccion23 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta18, Miembro = lucia };

            var reaccion24 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta19, Miembro = franco };
            var reaccion25 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta19, Miembro = agustin };
            var reaccion26 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta20, Miembro = sofia };
            var reaccion27 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta20, Miembro = lucia };

            var reaccion28 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta21, Miembro = juan };
            var reaccion29 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta22, Miembro = franco };
            var reaccion30 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta22, Miembro = sofia };

            var reaccion31 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta23, Miembro = juan };
            var reaccion32 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta24, Miembro = agustin };
            var reaccion33 = new Reaccion { Tipo = TipoReaccion.MeGusta, Respuesta = respuesta24, Miembro = lucia };

            var reaccion34 = new Reaccion { Tipo = TipoReaccion.MeEncanta, Respuesta = respuesta25, Miembro = sofia };
            var reaccion35 = new Reaccion { Tipo = TipoReaccion.NoMeGusta, Respuesta = respuesta25, Miembro = franco };

            context.Reacciones.AddRange(
                reaccion1, reaccion2, reaccion3, reaccion4, reaccion5,
                reaccion6, reaccion7, reaccion8, reaccion9, reaccion10,
                reaccion11, reaccion12, reaccion13, reaccion14, reaccion15,
                reaccion16, reaccion17, reaccion18, reaccion19, reaccion20,
                reaccion21, reaccion22, reaccion23, reaccion24, reaccion25,
                reaccion26, reaccion27, reaccion28, reaccion29, reaccion30,
                reaccion31, reaccion32, reaccion33, reaccion34, reaccion35
            );

            await context.SaveChangesAsync();

            #endregion

        }

        
    }

}

