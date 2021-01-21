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
using Quartz.Impl;
using SmartFridgeApp.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartFridgeApp.API.Middleware;
using Quartz.Spi;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.API.Configuration.Modules;

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

            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]);
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSwaggerGen();

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

          
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
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
