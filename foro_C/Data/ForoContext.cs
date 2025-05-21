using foro_C.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace foro_C.Data
{
    public class ForoContext:DbContext
    {
        /*
         * La unidad de trabajo con la base de datos
          El punto de entrada a todas las entidades (DbSet<>)
          Lo que permite hacer: Add, Update, Remove, Find, etc.
           Y lo que traduce clases a tablas con EF Core
         * */


        public ForoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave compuesta
            modelBuilder.Entity<Habilitacion>()
                .HasKey(h => new { h.MiembroId, h.EntradaId });

            // Relación con Miembro
            modelBuilder.Entity<Habilitacion>()
                .HasOne(h => h.Miembro)
                .WithMany(m => m.Habilitaciones)
                .HasForeignKey(h => h.MiembroId);

            // Relación con Entrada
            modelBuilder.Entity<Habilitacion>()
                .HasOne(h => h.Entrada)
                .WithMany(e => e.Habilitaciones)
                .HasForeignKey(h => h.EntradaId);

            //Cada Miembro puede tener muchas habilitaciones
            //Cada Entrada puede tener muchas habilitaciones
        }

        

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Reaccion> Reacciones { get; set; }
        public DbSet<Habilitacion> Habilitaciones { get; set; }
      
        public DbSet<foro_C.Models.Persona> Persona { get; set; }
        public DbSet<Interaccion> Interaccion { get; set; }
    }
}
