using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SolarMonitor.Data.Repositories.MySql;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // this should take us to the build directory where the SolarMonitor.Web.Api 
            // config file is copied to
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(envName))
            {
                envName = "Development";
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                //.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{envName}.json", false, false)
                .AddUserSecrets<ApplicationDbContextFactory>()
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connString = configuration.GetConnectionString("DefaultConnection");
            const string dbPasswordKey = "MySqlSolarMonitorPassword";
            connString = connString.Replace($"@@{dbPasswordKey}@@",
                configuration[dbPasswordKey]);

            System.Console.WriteLine("About to register db context, conn string: " + connString);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connString, b => b.MigrationsAssembly("SolarMonitor.Data.Repositories.MySql"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
