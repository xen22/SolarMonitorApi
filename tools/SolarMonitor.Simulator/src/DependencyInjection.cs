using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarMonitor.Data.Repositories.MySql;

namespace SolarMonitor.Simulator
{
    public static class DependencyInjection
    {
        public static IServiceProvider GetServiceProvider()
        {
            var connString = "Server=localhost;Uid=root;Pwd=@@MySqlRootPassword@@;Database=SolarMonitorDb;";
            // string connString = Configuration.GetConnectionString("DefaultConnection");
            //            const string dbPasswordKey = "MySqlRootPassword";
            // connString = connString.Replace($"@@{dbPasswordKey}@@",
            //     Configuration[dbPasswordKey]);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connString));

            //serviceProvider.AddScoped<ILogger, >();

            return serviceProvider.BuildServiceProvider();


        }
    }
}
