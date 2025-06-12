using foro_C.Data;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace foro_C.Models.helperPrecarga
{
    public class PrecargaInMemory
    {
        public static async Task EnviarPrecargaAsync(ForoContext context, RoleManager<Rol> roleManager, UserManager<Persona> userManager)
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
                await userManager.CreateAsync(emp, "Empleado123!");
                await userManager.AddToRoleAsync(emp, "Administrador");
            }

            // Crear Miembros
            var miembros = new List<Miembro>
        {
            new() { UserName = "iron.agus", Nombre = "Agustín", Apellido = "Quinteros", Email = "agus@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 10101010 },
            new() { UserName = "gamer.ari", Nombre = "Ariel", Apellido = "Mendoza", Email = "ari@ort.edu.ar", FechaAlta = DateTime.Now, Telefono = 20202020 },
            // (agregá los demás miembros igual que antes...)
        };

            foreach (var miembro in miembros)
            {
                await userManager.CreateAsync(miembro, "Miembro123!");
                await userManager.AddToRoleAsync(miembro, "Miembro");
            }

            // El resto de las entidades (categorías, entradas, preguntas, etc.) dejalas como están en tu clase actual, después de crear los usuarios.

            context.SaveChanges();
        }
    }

}
