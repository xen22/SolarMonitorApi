using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AD.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using SolarMonitor.Data.Repositories;
using SolarMonitor.Data.Repositories.MySql;
using SolarMonitorApi.Filters;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.ModelBinders;
using SolarMonitorApi.Services;
using SolarMonitorApi.Validators;
using Swashbuckle.AspNetCore.Swagger;

using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace SolarMonitorApi.Configuration
{
    static class IServiceCollectionExtensions
    {
        private static string GetSwaggerXMLPath()
        {
            var app = PlatformServices.Default.Application;
            return System.IO.Path.Combine(app.ApplicationBasePath, "SolarMonitor.Web.Api.xml");
        }
        public static IServiceCollection AddCustomisedSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Solar Monitor API", Version = "v1" });
                //Set the comments path for the swagger json and ui.
                c.IncludeXmlComments(GetSwaggerXMLPath());

                // this is needed because we have classes with the same name (in different namespaces) used in Controller actions
                c.CustomSchemaIds(t => t.FullName);

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme()
                {
                    Type = "oauth2",
                        Flow = "password", //"accessCode", //"implicit",
                        AuthorizationUrl = "http://localhost:5000/connect/authorize",
                        TokenUrl = "http://localhost:5000/connect/token",
                        Scopes = new Dictionary<string, string>
                        { { "SolarMonitorApi", "Access to Solar Monitor API" },
                        }
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.DocumentFilter<SwaggerBasicAuthDocumentFilter>();
                c.OperationFilter<SwaggerDefaultValues>();
                c.OperationFilter<SwaggerInsertUnauthorized>();
                c.DescribeAllEnumsAsStrings();
            });

            return services;
        }

        public static IMvcBuilder AddCustomisedMvc(this IServiceCollection services)
        {
            return services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DeviceModelBinderProvider());
                options.ModelBinderProviders.Insert(1, new SensorModelBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static IServiceCollection AddCustomisedCors(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("AllowLocalhost8088", policy => policy.WithOrigins("http://localhost:8088"));
                option.AddPolicy("AllowAllOrigins", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                option.AddPolicy("AllowGetMethod", policy => policy.WithMethods("GET"));
            });

            return services;
        }

        public static IServiceCollection AddJwtTokenAuthentication(this IServiceCollection services,
            IConfigurationSection tokenConfig)
        {

            services.AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    //config.Authority = "http://localhost:5000";
                    config.Audience = "SolarMonitorApi";
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new X509SecurityKey(new X509Certificate2(tokenConfig["PublicKeyFile"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = "http://localhost:5000",

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = "SolarMonitorApi",

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here:
                        //ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        // public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        // {
        //     am.Mapper.Initialize(config =>
        //     {
        //         config.AddProfile<SiteProfile>();
        //         config.AddProfile<DeviceProfile>();
        //         config.AddProfile<SensorProfile>();
        //     });

        //     services.AddSingleton(am.Mapper.Configuration);
        //     services.AddScoped<am.IMapper>(sp =>
        //         new am.Mapper(sp.GetRequiredService<am.IConfigurationProvider>(), sp.GetService));

        //     services.AddTransient<DeviceTypeMemberResolver>();
        //     services.AddTransient<DeviceSiteMemberResolver>();
        //     services.AddTransient<SensorTypeMemberResolver>();
        //     services.AddTransient<SensorSiteMemberResolver>();

        //     return services;
        // }

        public static IServiceCollection AddCustomDependencyInjectionTypes(
            this IServiceCollection services,
            IConfigurationRoot config)
        {
            // IoC services configuration for custom interfaces

            // main configuration object
            services.AddSingleton<IConfiguration>(config);

            // enable use of Configuration options
            services.AddOptions();

            // services
            services.AddSingleton<ISiteService, SiteService>();
            services.AddSingleton<IDevicesService, DeviceService>();
            services.AddSingleton<IDeviceTypeService, DeviceTypeService>();
            services.AddSingleton<ISensorsService, SensorsService>();
            services.AddSingleton<ISensorTypeService, SensorTypeService>();
            services.AddSingleton<IMeasurementsService, MeasurementsService>();

            // other
            services.AddSingleton<IDateTime, DateTimeAdapter>();

            return services;
        }

    }
}