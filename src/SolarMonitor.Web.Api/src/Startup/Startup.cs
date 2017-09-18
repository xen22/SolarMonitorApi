using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using SolarMonitor.Data.Repositories.MySql;
using System;
using System.Threading.Tasks;
using System.IO;
using SolarMonitorApi.Configuration;
using SolarMonitor.Data.Adapters.AutoMapper;

namespace SolarMonitorApi
{
    /// <summary>
    /// The Startup class for the service - configures services, DI, logging, Swagger, etc.
    /// </summary>
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        : this(env)
        {
            _loggerFactory = loggerFactory;
        }

        /// Constructor - this is used to set up and build the configuration for the Web API server.
        public Startup(IHostingEnvironment env)
        {
            // TODO: change path so that it works on Windows (i.e when deployed to Azure)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile("/tmp/logs/SolarMonitor_log-{Date}.txt").CreateLogger();

            Configuration = ConfigurationFactory.GetConfiguration(
                env.EnvironmentName, env.ContentRootPath);
        }


        /// Hook for IntegrationTests
        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            string connString = Configuration.GetConnectionString("DefaultConnection");
            const string dbPasswordKey = "MySqlSolarMonitorPassword";
            connString = connString.Replace($"@@{dbPasswordKey}@@",
                Configuration[dbPasswordKey]);

            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseMySql(connString, b => b.MigrationsAssembly("SolarMonitor.Web.Api")));
        }

        /// Hook for IntegrationTests 
        protected virtual void EnsureDatabaseCreated(ApplicationDbContext dbContext)
        {

        }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddCustomisedCors()
                .AddCustomisedSwaggerGen()
                .AddCustomDependencyInjectionTypes(Configuration)
                .AddJwtTokenAuthentication(Configuration.GetSection("Tokens"))
                .AddAutoMapper()
                .AddMySqlRepositories()
                .AddCustomisedMvc();

            ConfigureDatabase(services);

            return services.BuildServiceProvider();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging")).AddSerilog();
            loggerFactory.AddDebug().AddSerilog();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                EnsureDatabaseCreated(dbContext);
            }

            app
                .UseDevelopmentOptions(env)
                .UseNginx()
                .UseStaticFiles()
                // Note:  BasicAuthentication middleware must come before the built-in 
                // Authentication (which uses JWT tokens)
                //.UseBasicAuthentication()
                .UseAuthentication()
                .UseCustomisedSwagger(Configuration)
                .UseMvc();

        }
    }
}
