using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartFridgeApp.API.Configuration
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            // FrontendUrl is supplied via appsettings or GCP Secret Manager env var at runtime
            var frontendUrl = configuration["FrontendUrl"] ?? "http://localhost:3000";

            services.AddCors(options =>
            {
                options.AddPolicy("Development_Policy", builder =>
                    builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());

                options.AddPolicy("Production_Policy", builder =>
                    builder
                        .WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }
    }
}
