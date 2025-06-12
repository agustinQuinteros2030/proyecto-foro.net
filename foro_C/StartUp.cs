using foro_C.Data;
using foro_C.Models;
using foro_C.Models.helperPrecarga;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace foro_C
{
    public static class StartUp
    {
        public static WebApplication InicializarApp(String[] args)
        {
            //creamos instancia de nuestro servidor web
            var builder = WebApplication.CreateBuilder(args);

            ConfiguresServices(builder);//lo configuramos con sus servicios 
            var app = builder.Build();//despues configuramos los middelware 
            ConfigureAsync(app);


            return app;
        }

        private static void ConfiguresServices(WebApplicationBuilder builder)
        {
            //tenemos configurado el entorno de bd

            string hostname = Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "localhost";

            if (hostname is "DESKTOP-773F6PF")
            {
                builder.Services.AddDbContext<ForoContext>(options => options.UseInMemoryDatabase("MiDB"));
            }
            else
            {
                builder.Services.AddDbContext<ForoContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("foroDBCS"));

                });
            }

            #region Configuración de Identity

            builder.Services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<ForoContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false; // No requiere caracteres especiales
                options.Password.RequireLowercase = false; // No requiere letras minúsculasAdd commentMore actions
                options.Password.RequireUppercase = false; // No requiere letras mayúsculas
                options.Password.RequireDigit = false; // No requiere dígitos
                options.Password.RequiredLength = 6; // Longitud mínima de la contraseña
            });
            #endregion

            builder.Services.AddControllersWithViews();

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/Account/IniciarSesion";
                opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                opciones.Cookie.Name = "IdentidadForoApp";
            });
        }

        private static async Task ConfigureAsync(WebApplication app)
        {


            // Se ejecuta antes que empiece la app
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ForoContext>();

                var userManager = services.GetRequiredService<UserManager<Persona>>();
                var roleManager = services.GetRequiredService<RoleManager<Rol>>();
                await PrecargaInMemory.EnviarPrecargaAsync(context, roleManager, userManager);



                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }
                   
            }

            // Middleware HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication(); // autenticamos antes de autorizar
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }



}
