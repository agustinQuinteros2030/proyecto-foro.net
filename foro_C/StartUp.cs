using foro_C.Data;
using foro_C.Models.helperPrecarga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace foro_C
{
    public static class StartUp
    {
        public static WebApplication inicializarApp (String[] args)
        {
            //creamos instancia de nuestro servidor web
            var builder = WebApplication.CreateBuilder(args);

            ConfiguresServices(builder);//lo configuramos con sus servicios 
            var app = builder.Build();//despues configuramos los middelware 
            Configure(app);
           

            return app;
        }

      private static void ConfiguresServices(WebApplicationBuilder builder)
        {
            //tenemos configurado el entorno de bd
            builder.Services.AddDbContext<ForoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("foro1c7")));
           

            builder.Services.AddControllersWithViews();

        }

      private static void Configure(WebApplication app)
        {
            
        
            // Se ejecuta antes que empiece la app
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ForoContext>();
               
                    Precarga.EnviarPrecarga(context);
              
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
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }


        
    }
