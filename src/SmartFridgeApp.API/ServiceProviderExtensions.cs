using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFridgeApp.API.Quartz;
using SmartFridgeApp.API.Services;
using SmartFridgeApp.Core.Application.Services;
using SmartFridgeApp.Core.Contracts;
using SmartFridgeApp.Core.Contracts.Auth;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Core.Contracts.ExternalRecipes;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.ExternalRecipes;
using SmartFridgeApp.Infrastructure.Database;
using SmartFridgeApp.Infrastructure.Translation;
using SmartFridgeApp.Infrastructure.Auth;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.KitchenMembers;
using SmartFridgeApp.Infrastructure.Kitchens;
using SmartFridgeApp.Infrastructure.Inventory;
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
        var connectionString = configuration[$"{Consts.SmartFridgeAppConnectionStringLabel}:ConnectionString"];
        services.Configure<DatabaseOptions>(configuration.GetSection(Consts.SmartFridgeAppConnectionStringLabel));

        // Marten event store (inventory)
        services.AddMartenEventStore(connectionString);

        // Infrastructure
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IKitchenRepository, KitchenRepository>();
        services.AddScoped<IKitchenMemberRepository, KitchenMemberRepository>();
        services.AddScoped<IFoodProductRepository, FoodProductRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IRecipeFinderService, RecipeFinderService>();
        services.AddScoped<INotifier, EmailSender>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

        // Application services
        services.AddScoped<IKitchenService, KitchenService>();
        services.AddScoped<IFoodProductService, FoodProductService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IKitchenMemberService, KitchenMemberService>();
        services.AddScoped<IShoppingListService, ShoppingListService>();
        // services.AddScoped<IRecipeImportService, RecipeImportService>(); // disabled: recipe import feature

        // External recipe sources
        services.Configure<SpoonacularOptions>(configuration.GetSection("Spoonacular"));
        services.AddHttpClient<IExternalRecipeSource, SpoonacularRecipeSource>();

        // Translation (disabled: only used by recipe import)
        // services.Configure<TranslationOptions>(configuration.GetSection("Translation"));
        // services.AddHttpClient<ITranslationService, LibreTranslateService>();

        // Notification handlers
        services.AddScoped<IDomainEventNotificationHandler<KitchenAddedNotification>, KitchenAddedNotificationHandler>();
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