using foro_C.Data;
using foro_C.Models;
using foro_C.Models.helperPrecarga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace foro_C
{
    public static class StartUp
    {
        public static WebApplication InicializarApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfiguresServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }

        private static void ConfiguresServices(WebApplicationBuilder builder)
        {
            string hostname = Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "localhost";

            if (hostname is not "DESKTOP-773F6PF")
            {
                builder.Services.AddDbContext<ForoContext>(options =>
                    options.UseInMemoryDatabase("MiDB"));
            }
            else
            {
                builder.Services.AddDbContext<ForoContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("foroDBCS")));
            }

            builder.Services.AddIdentity<Persona, IdentityRole<int>>()
                .AddEntityFrameworkStores<ForoContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            });

            builder.Services.AddControllersWithViews();
        }

        private static void Configure(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ForoContext>();

                string proveedor = context.Database.ProviderName ?? "";

                if (proveedor.Contains("InMemory", StringComparison.OrdinalIgnoreCase))
                {
                    PrecargaInMemory.EnviarPrecarga(context);
                }
                else
                {
                    try
                    {
                        context.Database.Migrate();
                        Precarga.EnviarPrecarga(context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("⚠ Error al conectar con SQL Server:");
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }


    } 
    }
