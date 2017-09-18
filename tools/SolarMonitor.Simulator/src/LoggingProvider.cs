using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace SolarMonitor.Simulator
{
    public class LoggingProvider
    {
        IServiceProvider _serviceProvider;
        public LoggingProvider(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        public ILogger GetLogger()
        {
            _serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = _serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            return logger;
        }
    }
}