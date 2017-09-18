using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolarMonitor.Data.Repositories.MySql;

namespace SolarMonitor.Simulator
{
    public class Program
    {
        void ReCreateDb()
        {

        }

        public static void Main(string[] args)
        {
            var serviceProvider = DependencyInjection.GetServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogInformation("SolarMonitorSimulator version " + Assembly.GetEntryAssembly().GetName().Version);

            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            CommandArgument names = null;
            commandLineApplication.Command("name",
                (target) =>
                names = target.Argument(
                    "fullname",
                    "Enter the full name of the person to be greeted.",
                    multipleValues: true));
            CommandOption recreateDb = commandLineApplication.Option(
                "-r | --recreate-db", "Drop and re-create database (SolarMonitorDb).",
                CommandOptionType.NoValue);
            commandLineApplication.HelpOption("-? | -h | --help");


            commandLineApplication.OnExecute(() =>
            {
                if (recreateDb.HasValue())
                {
                    Console.WriteLine("Re-creating db.");
                    //ReCreateDb();
                }
                return 0;
            });
            commandLineApplication.Execute(args);
        }
    }
}
