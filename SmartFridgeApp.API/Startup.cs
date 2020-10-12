﻿using System;
using System.Reflection;
using Autofac;
using MediatR;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using SmartFridgeApp.API.InternalCommands;
using SmartFridgeApp.API.Modules;
using SmartFridgeApp.API.Outbox;
using SmartFridgeApp.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace SmartFridgeApp.API
{
    public class Startup
    {
        private const string SmartFridgeAppConnectionString = "SmartFridgeAppConnectionString";

        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

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
            services.AddCors(options =>
            {
                options.AddPolicy("CORS_Policy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3001",
                                        "http://localhost:3000")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin();
                });
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSwaggerGen();

            services
               .AddEntityFrameworkSqlServer()
               .AddDbContext<SmartFridgeAppContext>(options =>
               {
                   options
                       .UseSqlServer(this.Configuration[SmartFridgeAppConnectionString]);
               });

            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CORS_Policy");
            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            ConfigureSwagger(app);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            this._schedulerFactory = new StdSchedulerFactory();
            this._scheduler = _schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            builder.RegisterModule(new InfrastructureModule(Configuration[SmartFridgeAppConnectionString]));
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new OutboxModule());
            builder.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<SmartFridgeAppContext>();
                dbContextOptionsBuilder.UseSqlServer(this.Configuration[SmartFridgeAppConnectionString]);

                return new SmartFridgeAppContext(dbContextOptionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();


            // TODO: new JobFactory(IContainer) -< how to inject IContainer here? it was upgrade to net core 3.1 and now doesnt works

            //_scheduler.JobFactory = new JobFactory();
            //_scheduler.Start().GetAwaiter().GetResult();

            //var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            //var trigger =
            //    TriggerBuilder
            //        .Create()
            //        .StartNow()
            //        .WithCronSchedule("0/15 * * * * ?")
            //        .Build();

            //_scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

            //var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            //var triggerCommandsProcessing =
            //    TriggerBuilder
            //        .Create()
            //        .StartNow()
            //        .WithCronSchedule("0/15 * * * * ?")
            //        //.WithCronSchedule("0 0/1 * * * ?")
            //        .Build();
            //_scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();

        }
        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(
                c => c.SerializeAsV2 = true
            );

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFridgeApp API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
