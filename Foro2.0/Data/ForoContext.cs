using Microsoft.EntityFrameworkCore;
using Foro2._0.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ForoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public ForoContext(DbContextOptions<ForoContext> options) : base(options)
    {
    }

    public DbSet<Persona> Personas { get; set; }
    public DbSet<Miembro> Miembros { get; set; }
    public DbSet<Empleado> Empleados { get; set; }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<Pregunta> Preguntas { get; set; }
    public DbSet<Respuesta> Respuestas { get; set; }
    public DbSet<Reaccion> Reacciones { get; set; }
    public DbSet<Habilitacion> Habilitaciones { get; set; }

    public DbSet<Interacion> Interaciones { get; set; }
    public DbSet<Rol> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // evitan que se repitan categorías
        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nombre)
            .IsUnique();


        modelBuilder.Entity<Entrada>()
            .HasOne(e => e.Categoria)
            .WithMany(c => c.Entradas)
            .HasForeignKey(e => e.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
    }
}


