using foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace foro_C.Data
{
    public class ForoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
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


            #region  Establecer Nombres para los Identity Stores
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas"); //resuelve el proble de ASPNETUSERS
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonaRoles");
            #endregion

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

        public DbSet<Rol> Roles { get; set; }
    }
}
