using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SolarMonitorApi.Configuration;

namespace SolarMonitorApi
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = ConfigurationFactory.GetConfiguration(envName, Directory.GetCurrentDirectory());

            var pfxFilename = config["CertificatePfxFile"] ?? "testCert.pfx";
            //var pfxFile = Path.Combine(pfxFilename);

            // TODO: move password to a secrets file
            string pfxPwd = envName == "Development" ? config["PfxCertificatePassword"] : "";

            CreateWebHostBuilder(args, config).Build().Run();

            // Old code (from ASP.NET Core 1.1, not needed in 2.0)
            // var host = new WebHostBuilder()
            //     .UseKestrel(options =>
            //     {
            //         // options.UseHttps(pfxFilename, pfxPwd);
            //         // options.UseConnectionLogging();
            //     })
            //     // Note: URLs are set:
            //     // - in .vscode/launch.json (see ASPNETCORE_URLS under the ".NET Core Launch (web)" config, when running under debugger)
            //     // - in appsettings.Development.json (under development environment)
            //     // - in appsettings.json (under production environment)
            //     // - in src/Dockerfile (see ASPNETCORE_URLS) for Docker images created by Jenkins
            //     // override here for testing: 
            //     //.UseUrls("https://*:5001")
            //     .UseConfiguration(config)
            //     .UseContentRoot(Directory.GetCurrentDirectory())
            //     // Note: we use nginx as the reverse-proxy in production 
            //     // Enable next line (and add Microsoft.AspNetCore.Server.IISIntegration to project.json) for Azure.
            //     //.UseIISIntegration()
            //     .UseStartup<Startup>()
            //     .Build();

            // host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot config) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseConfiguration(config)
            .UseContentRoot(Directory.GetCurrentDirectory());
    }
}