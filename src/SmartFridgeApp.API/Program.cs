using System;
using System.Net;
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartFridgeApp.API.Configuration;
using SmartFridgeApp.API.Middleware;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.API.Services;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API;

public class Program
{
    private const string SmartFridgeAppConnectionString = "SmartFridgeAppOptions:ConnectionString";

    public static void Main(string[] args)
    {
        // Npgsql 6+ rejects DateTime with Kind=UTC for 'timestamp without time zone'.
        // Legacy mode restores the old behaviour of writing UTC datetimes as-is.
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();

        // Trust the X-Forwarded-Proto / X-Forwarded-For headers from Cloud Run's load balancer
        // so ASP.NET Core uses https:// when building OAuth redirect URIs.
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            // Cloud Run LB is outside the default loopback-only range — clear the defaults
            // so all forwarded headers are trusted.
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        builder.Services.AddRazorPages();
        builder.Services.AddHealthChecks();

        builder.Services.ConfigureCors(builder.Configuration);
        builder.Services.ConfigureJwt(builder.Configuration);
        builder.Services.ConfigureGoogle(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.ConfigureRateLimiting(builder.Configuration);

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

        if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Docker"))
        {
            builder.Services.AddHostedService<DevInventorySeeder>();
        }

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
        // MUST be first — rewrites scheme/host before any other middleware reads them
        app.UseForwardedHeaders();

        // IP rate limiting — early in pipeline to reject before heavy processing
        app.UseIpRateLimiting();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("Development_Policy");
        }
        else
        {
            app.UseHsts();
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
        app.UseHttpsRedirection();

        var defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("index.html");
        app.UseDefaultFiles(defaultFilesOptions);
        app.UseStaticFiles();

        app.MapControllers();
        app.MapRazorPages();
        app.MapHealthChecks("/healthcheck");

        app.Run();
    }
}
