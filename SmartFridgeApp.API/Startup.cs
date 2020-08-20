using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using SmartFridgeApp.API.InternalCommands;
using SmartFridgeApp.API.Modules;
using SmartFridgeApp.API.Outbox;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API
{
    public class Startup
    {
        private const string SmartFridgeAppConnectionString = "SmartFridgeAppConnectionString";

        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        public Startup(IHostingEnvironment env)
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
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

            services
                .AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.Converters.Add(
                    new Newtonsoft.Json.Converters.StringEnumConverter()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            this.AddSwagger(services);

            services
               .AddEntityFrameworkSqlServer()
               .AddDbContext<SmartFridgeAppContext>(options =>
               {
                   options
                       .UseSqlServer(this.Configuration[SmartFridgeAppConnectionString]);
               });
            
            return CreateAutofacServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CORS_Policy");

            app.UseHttpsRedirection();
            app.UseMvc();

            this.StartQuartz(serviceProvider);

            ConfigureSwagger(app);
        }

        private IServiceProvider CreateAutofacServiceProvider(IServiceCollection services)
        {
            var container = new ContainerBuilder();

            container.Populate(services);
            container.RegisterModule(new InfrastructureModule(Configuration[SmartFridgeAppConnectionString]));
            container.RegisterModule(new MediatorModule());

            var buildContainer = container.Build();

            //ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            return new AutofacServiceProvider(buildContainer);
        }

        public void StartQuartz(IServiceProvider serviceProvider)
        {
            this._schedulerFactory = new StdSchedulerFactory();
            this._scheduler = _schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            var container = new ContainerBuilder();

            container.RegisterModule(new OutboxModule());
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new InfrastructureModule(Configuration[SmartFridgeAppConnectionString]));

            container.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<SmartFridgeAppContext>();
                dbContextOptionsBuilder.UseSqlServer(this.Configuration[SmartFridgeAppConnectionString]);

                //dbContextOptionsBuilder
                //    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new SmartFridgeAppContext(dbContextOptionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            _scheduler.JobFactory = new JobFactory(container.Build());

            _scheduler.Start().GetAwaiter().GetResult();

            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            var trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * * * ?")
                    .Build();

            _scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            var triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * * * ?")
                    //.WithCronSchedule("0 0/1 * * * ?")
                    .Build();
            _scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();
        }

        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFridgeApp API V1");
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "SmartFridgeApp",
                    Version = "v1",
                    Description = "Smart Fridge Application.",
                });
            });
        }
    }
}
