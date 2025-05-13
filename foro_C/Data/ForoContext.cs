using foro_C.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace foro_C.Data
{
    public class ForoContext:DbContext
    {
        public ForoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Habilitacion>()
                .HasKey(h => new { h.MiembroId, h.EntradaId });
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Reaccion> Reacciones { get; set; }
        public DbSet<Habilitacion> Habilitaciones { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<foro_C.Models.Persona> Persona { get; set; }
    }
}
