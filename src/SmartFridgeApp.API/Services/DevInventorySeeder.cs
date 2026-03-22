using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartFridgeApp.Core.Domain.Inventory;
using SmartFridgeApp.Core.Domain.Inventory.Events;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.API.Services;

public class DevInventorySeeder(IServiceProvider serviceProvider) : IHostedService
{
    // Kitchen IDs matching 06-dev-seed.sql
    private static readonly Guid KitchenJan = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000001");
    private static readonly Guid KitchenAnna = Guid.Parse("b2c3d4e5-0000-0000-0000-000000000002");
    private static readonly Guid KitchenDariusz = Guid.Parse("c3d4e5f6-0000-0000-0000-000000000003");

    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        await using var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

        // Skip if events already exist for any kitchen
        var existing = await session.Events.FetchStreamStateAsync(KitchenJan, ct);
        if (existing is not null) return;

        SeedKitchen(session, KitchenJan, memberId: 1, [
            (1, 500f, Unit.Grams, 7, "Stek wołowy", StorageLocation.Fridge, [ItemTag.HighProtein]),
            (22, 1000f, Unit.Mililiter, 5, "Mleko 3.2%", StorageLocation.Fridge, []),
            (19, 10f, Unit.Pieces, 14, "Jaja L", StorageLocation.Fridge, [ItemTag.Organic]),
            (27, 200f, Unit.Grams, 21, "Ser żółty", StorageLocation.Fridge, []),
            (39, 2000f, Unit.Grams, 30, "Ziemniaki", StorageLocation.Pantry, []),
            (62, 300f, Unit.Grams, 5, "Brokuł", StorageLocation.Fridge, [ItemTag.Vegan]),
        ]);

        SeedKitchen(session, KitchenAnna, memberId: 2, [
            (15, 400f, Unit.Grams, 3, "Pierś z kurczaka", StorageLocation.Fridge, [ItemTag.HighProtein]),
            (31, 500f, Unit.Mililiter, 10, "Jogurt naturalny", StorageLocation.Fridge, [ItemTag.Organic]),
            (44, 300f, Unit.Grams, 6, "Pomidory", StorageLocation.Fridge, [ItemTag.Vegan]),
            (58, 100f, Unit.Grams, 60, "Czosnek", StorageLocation.Pantry, []),
            (73, 250f, Unit.Grams, 3, "Truskawki", StorageLocation.Fridge, [ItemTag.Organic, ItemTag.Vegan]),
        ]);

        // Dariusz stocks items in Anna's kitchen (memberId 3) and also in his own
        SeedKitchen(session, KitchenAnna, memberId: 3, [
            (8, 500f, Unit.Grams, 4, "Mięso mielone", StorageLocation.Freezer, []),
            (60, 300f, Unit.Grams, 14, "Cebula", StorageLocation.Pantry, [ItemTag.Vegan]),
        ]);

        SeedKitchen(session, KitchenDariusz, memberId: 3, [
            (18, 300f, Unit.Grams, 7, "Boczek", StorageLocation.Fridge, [ItemTag.HighProtein]),
            (14, 500f, Unit.Grams, 10, "Kiełbasa", StorageLocation.Fridge, []),
            (20, 150f, Unit.Grams, 14, "Mozzarella", StorageLocation.Fridge, []),
            (41, 200f, Unit.Grams, 5, "Szpinak", StorageLocation.Fridge, [ItemTag.Vegan, ItemTag.GlutenFree]),
            (7, 800f, Unit.Grams, 4, "Karkówka na grilla", StorageLocation.Freezer, [ItemTag.ForParty]),
        ]);

        await session.SaveChangesAsync(ct);
        Console.WriteLine("[DevInventorySeeder] Seeded inventory events for 3 kitchens.");
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    private static void SeedKitchen(
        IDocumentSession session,
        Guid kitchenId,
        int memberId,
        List<(short foodProductId, float amount, Unit unit, int daysUntilExpiry, string note, StorageLocation location, List<ItemTag> tags)> items)
    {
        var events = new List<object>();
        foreach (var (foodProductId, amount, unit, daysUntilExpiry, note, location, tags) in items)
        {
            events.Add(new ItemStocked(
                Guid.NewGuid(),
                foodProductId,
                memberId,
                amount,
                unit,
                DateTimeOffset.UtcNow.AddDays(daysUntilExpiry),
                note,
                location,
                tags,
                DateTimeOffset.UtcNow));
        }

        session.Events.Append(kitchenId, events.ToArray());
    }
}
