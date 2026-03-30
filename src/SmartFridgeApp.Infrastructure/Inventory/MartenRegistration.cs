using JasperFx;
using JasperFx.Events.Projections;
using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Inventory;

namespace SmartFridgeApp.Infrastructure.Inventory;

public static class MartenRegistration
{
    public static IServiceCollection AddMartenEventStore(this IServiceCollection services, string connectionString)
    {
        services.AddMarten(opts =>
        {
            opts.Connection(connectionString);
            opts.DatabaseSchemaName = "inventory";

            opts.Projections.Snapshot<KitchenInventory>(SnapshotLifecycle.Inline);
            opts.Projections.Add<ActiveStockItemProjection>(ProjectionLifecycle.Inline);

            opts.AutoCreateSchemaObjects = AutoCreate.All;
        })
        .UseLightweightSessions();

        services.AddScoped<IKitchenInventoryRepository, KitchenInventoryRepository>();

        return services;
    }
}
