using Microsoft.Extensions.DependencyInjection;
using SolarMonitor.Data.Repositories;

namespace SolarMonitor.Data.Repositories.MySql
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlRepositories(this IServiceCollection services)
        {
            // repositories - scoped (per request) because DbContext is not thread-safe
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISensorRepository, SensorRepository>();
            services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
            services.AddScoped<ILoadTypeRepository, LoadTypeRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceTypeRepository, DeviceTypeRepository>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();

            return services;
        }
    }

}