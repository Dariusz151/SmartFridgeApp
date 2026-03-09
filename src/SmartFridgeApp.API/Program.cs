using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartFridgeApp.API.Configuration;
using SmartFridgeApp.API.Middleware;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API;

public class Program
{
    private const string SmartFridgeAppConnectionString = "SmartFridgeAppConnectionString:ConnectionString";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();

        builder.Services.AddRazorPages();
        builder.Services.AddHealthChecks();

        builder.Services.ConfigureCors(builder.Configuration);
        builder.Services.ConfigureJwt(builder.Configuration);
        builder.Services.ConfigureGoogle(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddHostedService<QuartzHostedService>();

        builder.Services.AddDbContext<SmartFridgeAppContext>(options =>
        {
            options.UseNpgsql(builder.Configuration[SmartFridgeAppConnectionString]);
        });

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<SmartFridgeAppContext>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database initialization failed. The app will continue starting up.");
            }
        }

        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("Development_Policy");
        }
        else
        {
            app.UseCors("Production_Policy");
        }

        app.UseSwagger(c => c.SerializeAsV2 = true);
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFridgeApp API");
            c.RoutePrefix = "docs";
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        var defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("index.html");
        app.UseDefaultFiles(defaultFilesOptions);
        app.UseStaticFiles();

        app.MapControllers();
        app.MapRazorPages();
        app.MapHealthChecks("/healthcheck");

        // Note: UseHttpsRedirection is intentionally omitted.
        // Cloud Run terminates TLS at the load balancer; the container only receives HTTP.
        // Enabling it here would cause redirect loops in production.

        app.Run();
    }
}
