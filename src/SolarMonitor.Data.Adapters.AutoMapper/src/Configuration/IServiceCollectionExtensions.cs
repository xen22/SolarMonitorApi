using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<SiteProfile>();
                config.AddProfile<DeviceProfile>();
                config.AddProfile<SensorProfile>();
                config.AddProfile<MeasurementProfile>();
            });

            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp =>
                new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            services.AddTransient<DeviceTypeMemberResolver>();
            services.AddTransient<DeviceSiteMemberResolver>();
            services.AddTransient<SensorTypeMemberResolver>();
            services.AddTransient<SensorTypeIdMemberResolver>();
            services.AddTransient<SensorSiteMemberResolver>();
            services.AddTransient<SensorSiteIdMemberResolver>();
            services.AddTransient<MeasurementSensorMemberResolver>();

            services.AddSingleton<ISiteAdapter, SiteAdapter>();
            services.AddSingleton<IDeviceAdapter, DeviceAdapter>();
            services.AddSingleton<ISensorAdapter, SensorAdapter>();
            services.AddSingleton<IMeasurementAdapter, MeasurementAdapter>();

            return services;
        }
    }
}