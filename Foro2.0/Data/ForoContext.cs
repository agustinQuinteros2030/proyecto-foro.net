using Microsoft.EntityFrameworkCore;
using Foro2._0.Models;
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

        modelBuilder.Entity<Persona>()
            .HasDiscriminator<string>("TipoUsuario")
            .HasValue<Persona>("Persona")
            .HasValue<Miembro>("Miembro")
            .HasValue<Empleado>("Empleado");

        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nombre)
            .IsUnique();

        modelBuilder.Entity<Entrada>()
            .HasOne(e => e.Categoria)
            .WithMany(c => c.Entradas)
            .HasForeignKey(e => e.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔥 Relación Habilitaciones -> Entrada (Cascade está OK)
        modelBuilder.Entity<Habilitacion>()
            .HasOne(h => h.Entrada)
            .WithMany(e => e.MiembrosHabilitados)
            .HasForeignKey(h => h.EntradaId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔥 Relación Habilitaciones -> Miembro (❌ NO Cascade)
        modelBuilder.Entity<Habilitacion>()
            .HasOne(h => h.Miembro)
            .WithMany(m => m.AccesoEntradasPrivadas)
            .HasForeignKey(h => h.MiembroId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
    }


}


