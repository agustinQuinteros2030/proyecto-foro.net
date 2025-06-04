using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace foro_C.Data
{
    public class ForoContextFactory : IDesignTimeDbContextFactory<ForoContext>
    {
        public ForoContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ForoContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("foroDBCS"));

            return new ForoContext(optionsBuilder.Options);
        }
    }
}