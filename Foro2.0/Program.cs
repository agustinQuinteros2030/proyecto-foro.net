using Foro2._0.Data;
using Foro2._0.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Foro2._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ForoContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(9, 3, 0)) // Cambia por tu versión de MySQL
                )
            );


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/IniciarSesion"; 

                options.AccessDeniedPath = "/Account/AccessDenied";
            });

        

            // Identity personalizado con Rol propio
            builder.Services.AddIdentity<Persona, Rol>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ForoContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Ejecutar creación de roles ANTES de levantar la app
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<Persona>>();
                var roleManager = services.GetRequiredService<RoleManager<Rol>>();
                var context = services.GetRequiredService<ForoContext>();

            


              //  Precarga.Seed(userManager, roleManager, context).GetAwaiter().GetResult();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // clave para identity
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        
            
    }
}






