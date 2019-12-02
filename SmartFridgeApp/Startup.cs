using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFridgeApp.Domain.Models;
using SmartFridgeApp.Domain.Repositories;
using SmartFridgeApp.Domain.Services;
using SmartFridgeApp.Identity;
using SmartFridgeApp.Infrastructe.Services;
using SmartFridgeApp.Infrastructure.Repositories;
using SmartFridgeApp.Persistence;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartFridgeApp
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:30010",
                                        "http://localhost:3000")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin();
                });
            });

            services.AddIdentity<User, UserRole>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUserStore<User>, Identity.UserStore>();
            services.AddTransient<IRoleStore<UserRole>, RoleStore>();

            services.AddScoped<IFoodItemService, FoodItemService>();
            services.AddScoped<IFoodItemRepository, FoodItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
            });
           
            services.AddDbContext<SmartFridgeContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:SmartFridgeDB"]));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Version = "v1",
                    Title = "Smart Fridge App API",
                    Description = "A simple app to manage your food items",
                    Contact = new Contact
                    {
                        Name = "Dariusz Koziol",
                        Email = "dariusz151.developer@gmail.com"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SignInManager<User> s)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (s.UserManager.FindByNameAsync("dariusz").Result == null)
                {
                    var result = s.UserManager.CreateAsync(new User{
                        UserName = "dariusz"
                    }, "Password#34").Result;
                }
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Fridge App API");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
