using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace SolarMonitor.Api.IntegrationTests
{
    class TestStartup : SolarMonitorApi.Startup
    {
        //public IConfigurationRoot TestConfiguration { get; }
        public TestStartup(IHostingEnvironment env, ILoggerFactory loggerFactory) : base(env, loggerFactory)
        {
            // // set up the test configuration
            // var envName = env.EnvironmentName; // "Development";
            // var settingsFile = $"appsettings.{envName}.json";
            // TestConfiguration = new ConfigurationBuilder()
            //     .AddJsonFile(settingsFile, optional: true, reloadOnChange: false)
            //     //.AddUserSecrets("SolarMonitorSecrets")
            //     // use the secrets from the main SolarMonitor.Web.Api project
            //     .AddUserSecrets<SolarMonitorApi.Startup>()
            //     .Build();
        }

        protected override void ConfigureDatabase(IServiceCollection services)
        {
            // We have 2 options here: 
            // 1. an in-memory Sqlite provider or 
            // 2. a more basic EFCore in-memory provider - this is faster but not suitable for testing 
            //   databases that make use of advanced SQL features

            // We'll use opt. 1 because the EF core in-memory provider has problems with inserting entities without explicit primary keys

            // Option 1 (Sqlite)
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            services
              .AddEntityFrameworkSqlite()
              .AddDbContext<Data.Repositories.MySql.ApplicationDbContext>(
                options => options.UseSqlite(connection)
              );

            // Option 2 (EF Core in-memory db)
            //services.AddDbContext<AgeRanger.Models.AgeRangerContext>(opt => 
            //    opt.UseInMemoryDatabase());

        }

        protected override void EnsureDatabaseCreated(Data.Repositories.MySql.ApplicationDbContext dbContext)
        {
            // these are necessary for the Sqlite provider only
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
        }

    }
}
