using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.API.Services;
using SmartFridgeApp.Core.Application.Services;
using SmartFridgeApp.Core.Contracts.Auth;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Services;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.Database;
using SmartFridgeApp.Infrastructure.Auth;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.FridgeItems;
using SmartFridgeApp.Infrastructure.FridgeMembers;
using SmartFridgeApp.Infrastructure.Fridges;
using SmartFridgeApp.Infrastructure.Notifications;
using SmartFridgeApp.Infrastructure.Recipes;
using SmartFridgeApp.Infrastructure.SeedWork;
using SmartFridgeApp.Shared;
using SmartFridgeApp.Shared.SeedWork;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace SmartFridgeApp.API;

public static class ServiceProviderExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(Consts.SmartFridgeAppConnectionStringLabel));

        // Infrastructure
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IFridgeRepository, FridgeRepository>();
        services.AddScoped<IFridgeMemberRepository, FridgeMemberRepository>();
        services.AddScoped<IFridgeItemRepository, FridgeItemRepository>();
        services.AddScoped<IFoodProductRepository, FoodProductRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IRecipeFinderService, RecipeFinderService>();
        services.AddScoped<INotifier, EmailSender>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddSingleton<IFridgeScoringPolicy, DefaultFridgeScoringPolicy>();

        // Application services
        services.AddScoped<IFridgeService, FridgeService>();
        services.AddScoped<IFoodProductService, FoodProductService>();
        services.AddScoped<IFridgeItemService, FridgeItemService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IFridgeMemberService, FridgeMemberService>();

        // Notification handlers
        services.AddScoped<IDomainEventNotificationHandler<FridgeAddedNotification>, FridgeAddedNotificationHandler>();
        services.AddScoped<IDomainEventNotificationHandler<RecipeAddedNotification>, RecipeAddedNotificationHandler>();

        // Quartz
        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<QuartzJobRunner>();
        services.AddTransient<ProcessOutboxJob>();
        services.AddTransient<CheckExpiringItemsJob>();
        services.AddSingleton(new JobSchedule(typeof(ProcessOutboxJob), "0 0/30 * * * ?"));
        services.AddSingleton(new JobSchedule(typeof(CheckExpiringItemsJob), "0 0 8 * * ?")); // daily at 8 AM

        return services;
    }
}