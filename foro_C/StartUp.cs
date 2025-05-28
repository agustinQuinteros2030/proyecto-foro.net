using foro_C.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddDbContext<ForoContext>(options => options.UseInMemoryDatabase("Foro"));
            builder.Services.AddControllersWithViews();

        }

      private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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