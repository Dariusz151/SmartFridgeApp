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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFridgeApp.API.Modules;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API
{
    public class Startup
    {
        private const string SmartFridgeAppConnectionString = "SmartFridgeAppConnectionString";

        public Startup(IHostingEnvironment env)
        {
            this.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Startup>()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            
            app.UseHttpsRedirection();
            app.UseMvc();

            ConfigureSwagger(app);
        }

        private IServiceProvider CreateAutofacServiceProvider(IServiceCollection services)
        {
            var container = new ContainerBuilder();

            container.Populate(services);
            container.RegisterModule(new InfrastructureModule(Configuration[SmartFridgeAppConnectionString]));

            var buildContainer = container.Build();

            //ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            return new AutofacServiceProvider(buildContainer);
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
