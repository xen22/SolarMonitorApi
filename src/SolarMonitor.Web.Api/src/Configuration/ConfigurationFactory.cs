using System.IO;
using Microsoft.Extensions.Configuration;

namespace SolarMonitorApi.Configuration
{
    class ConfigurationFactory
    {
        public static IConfigurationRoot GetConfiguration(string envName, string path)
        {
            var settingsFile = envName == "" ? "appsettings.json" : $"appsettings.{envName}.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                // Note: if reloadOnChange is set to true, we run into "System.IO.IOException : 
                // The configured user limit (128) on the number of inotify instances has been reached"
                // when running the integration tests on Linux.
                .AddJsonFile(settingsFile, optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddUserSecrets<ConfigurationFactory>();

            return builder.Build();
        }
    }
}
