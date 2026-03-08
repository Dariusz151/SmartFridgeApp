using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartFridgeApp.API.Configuration;
using SmartFridgeApp.API.Middleware;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API;

public class Program
{
    // Matches DatabaseOptions binding path in ServiceProviderExtensions.
    // Env var override: SmartFridgeAppConnectionString__ConnectionString=<value>
    private const string SmartFridgeAppConnectionString = "SmartFridgeAppConnectionString:ConnectionString";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuration
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();

        // Add services to the container
        builder.Services.AddRazorPages();
        builder.Services.AddHealthChecks();

        // CORS configuration
        builder.Services.ConfigureCors(builder.Configuration);

        // JWT authentication
        builder.Services.ConfigureJwt(builder.Configuration);

        // Google authentication
        builder.Services.ConfigureGoogle(builder.Configuration);

        // Infrastructure services
        builder.Services.AddInfrastructure(builder.Configuration);

        // Swagger/OpenAPI
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

        // Background services
        builder.Services.AddHostedService<QuartzHostedService>();

        // Database context
        builder.Services.AddDbContext<SmartFridgeAppContext>(options =>
        {
            options.UseNpgsql(builder.Configuration[SmartFridgeAppConnectionString]);
        });

        // Controllers with JSON options
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        var app = builder.Build();

        // Database initialization
        // Note: Database schema is created by Docker init scripts (.docker/db-init)
        // For development, the database will be auto-created on first run
        // For production, use proper migrations: dotnet ef migrations add <name>
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<SmartFridgeAppContext>();

            // Only ensure database exists (doesn't create schema if it already exists)
            // Comment this out if using migrations exclusively
            context.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline
        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("Development_Policy");
        }
        else
        {
            app.UseHsts();
            app.UseCors("Production_Policy");
        }

        // Swagger configuration
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
