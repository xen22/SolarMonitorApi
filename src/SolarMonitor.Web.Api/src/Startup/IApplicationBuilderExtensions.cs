using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;

namespace SolarMonitorApi.Configuration
{
    static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNginx(this IApplicationBuilder app)
        {
            // these are needed for Nginx reverse-proxy
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            return app;
        }

        public static IApplicationBuilder UseDevelopmentOptions(
            this IApplicationBuilder app,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                // Allow CORS in development so that we can test with web client running on a different domain
                // (including localhost with a different port)
                app.UseCors("AllowAllOrigins");
            }
            return app;
        }

        public static IApplicationBuilder UseCustomisedSwagger(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solar Monitor API v1");
                c.ConfigureOAuth2("swagger", config["AuthServiceSwaggerClientSecret"], "swagger-ui-realm", "Swagger UI");
                c.InjectOnCompleteJavaScript("/swagger-ui/client-credentials.js");
            });
            return app;
        }


    }
}
