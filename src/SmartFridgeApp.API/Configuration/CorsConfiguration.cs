using Microsoft.Extensions.DependencyInjection;

namespace SmartFridgeApp.API.Configuration
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Development_Policy",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                });
                options.AddPolicy("Production_Policy",
               builder =>
               {
                   builder
                       .WithOrigins("http://localhost:3000",
                                    "https://localhost:3001")
                       .AllowAnyHeader()
                       .AllowAnyMethod();

               });
            });
        }
    }
}
