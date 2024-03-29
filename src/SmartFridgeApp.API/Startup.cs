﻿using System;
using System.Reflection;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using FluentValidation;
using SmartFridgeApp.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using SmartFridgeApp.API.Middleware;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.API.Configuration.Modules;
using SmartFridgeApp.API.Configuration;
using FluentValidation.AspNetCore;
using SmartFridgeApp.Core.Application.Features.Fridges.AddFridge;
using SmartFridgeApp.Infrastructure.Validation;
using Microsoft.OpenApi.Models;

namespace SmartFridgeApp.API
{
    public class Startup
    {
        private const string SmartFridgeAppConnectionString = "SmartFridgeAppConnectionString";

        public Startup(IWebHostEnvironment env)
        {
            this.Configuration = new ConfigurationBuilder()
                .SetBasePath((env.ContentRootPath))
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddHealthChecks();
            services.ConfigureCors();
            services.ConfigureJwt(Configuration);
            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSwaggerGen(option =>
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
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            services.AddHostedService<QuartzHostedService>();

            services
               .AddEntityFrameworkSqlServer()
               .AddDbContext<SmartFridgeAppContext>(options =>
               {
                   options
                       .UseSqlServer(this.Configuration[SmartFridgeAppConnectionString]);
               });

            services
                .AddControllers()
                .AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

            AssemblyScanner.FindValidatorsInAssembly(typeof(AddFridgeCommandValidator).Assembly)
              .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CustomPipelineValidationBehavior<,>));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService <SmartFridgeAppContext> ();
                context.Database.Migrate();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("Development_Policy");
            }
            else
            {
                app.UseHsts();
                app.UseCors("Production_Policy");
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/healthcheck");
            });

            ConfigureSwagger(app);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new InfrastructureModule(Configuration[SmartFridgeAppConnectionString]));
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new OutboxModule());
            builder.RegisterModule(new QuartzModule());
            builder.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<SmartFridgeAppContext>();
                dbContextOptionsBuilder.UseSqlServer(this.Configuration[SmartFridgeAppConnectionString]);

                return new SmartFridgeAppContext(dbContextOptionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(
                c => c.SerializeAsV2 = true
            );

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"SmartFridgeApp API");
                c.RoutePrefix = "docs";
            });
        }
    }
}
