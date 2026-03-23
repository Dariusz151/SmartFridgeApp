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

        // ── Jan's kitchen (memberId 1) ──────────────────────────────────
        SeedKitchen(session, KitchenJan, memberId: 1, [
            (1,   500f, Unit.Grams,     7,  "Stek wołowy",       StorageLocation.Fridge,  [ItemTag.HighProtein], 49),  // Ribeye 300g
            (22,  1000f, Unit.Mililiter, 5,  "Mleko 3.2%",       StorageLocation.Fridge,  [], 1),                     // Łaciate 3,2%
            (19,  10f,  Unit.Pieces,    14,  "Jaja L",           StorageLocation.Fridge,  [ItemTag.Organic], 20),      // Jaja L 10szt.
            (27,  200f, Unit.Grams,     21,  "Ser żółty",        StorageLocation.Fridge,  [], 13),                     // Gouda
            (39,  2000f, Unit.Grams,    30,  "Ziemniaki",        StorageLocation.Pantry,  [], null),
            (62,  300f, Unit.Grams,      5,  "Brokuł",           StorageLocation.Fridge,  [ItemTag.Vegan], null),
            (149, 200f, Unit.Grams,     30,  "Masło",            StorageLocation.Fridge,  [], 9),                      // Kerrygold
            (109, 500f, Unit.Grams,      3,  "Chleb",            StorageLocation.Pantry,  [], 26),                     // Chleb pszenny krojony
            (90,  250f, Unit.Grams,    180,  "Kawa",             StorageLocation.Pantry,  [], 31),                     // Lavazza
            (95,  1000f, Unit.Mililiter, 14, "Sok pomarańczowy", StorageLocation.Fridge,  [], 34),                     // Cappy
            (134, 400f, Unit.Grams,      4,  "Łosoś filet",     StorageLocation.Fridge,  [ItemTag.HighProtein], 41),  // Łosoś atlantycki
            (121, 500f, Unit.Grams,    365,  "Makaron",          StorageLocation.Pantry,  [], 40),                     // Barilla Spaghetti
            (44,  500f, Unit.Grams,      5,  "Pomidory",         StorageLocation.Fridge,  [ItemTag.Vegan], null),
            (58,  100f, Unit.Grams,     60,  "Czosnek",          StorageLocation.Pantry,  [], null),
            (151, 500f, Unit.Mililiter, 365, "Oliwa z oliwek",   StorageLocation.Pantry,  [], null),
        ]);

        // ── Anna's kitchen — Anna (memberId 2) ─────────────────────────
        SeedKitchen(session, KitchenAnna, memberId: 2, [
            (15,  400f, Unit.Grams,      3, "Pierś z kurczaka", StorageLocation.Fridge,  [ItemTag.HighProtein], 22), // Cedrob
            (31,  500f, Unit.Mililiter, 10, "Jogurt naturalny", StorageLocation.Fridge,  [ItemTag.Organic], 5),     // Danone
            (44,  300f, Unit.Grams,      6, "Pomidory",         StorageLocation.Fridge,  [ItemTag.Vegan], null),
            (58,  100f, Unit.Grams,     60, "Czosnek",          StorageLocation.Pantry,  [], null),
            (73,  250f, Unit.Grams,      3, "Truskawki",        StorageLocation.Fridge,  [ItemTag.Organic, ItemTag.Vegan], null),
            (22,  1000f, Unit.Mililiter, 5, "Mleko 2%",         StorageLocation.Fridge,  [], 2),                    // Łaciate 2%
            (86,  100f, Unit.Grams,    180, "Czekolada",        StorageLocation.Pantry,  [], 12),                   // Lindt 70%
            (29,  200f, Unit.Mililiter, 14, "Śmietana 30%",     StorageLocation.Fridge,  [], 43),                   // Łaciata 30%
            (19,  6f,   Unit.Pieces,   14,  "Jaja XL",          StorageLocation.Fridge,  [ItemTag.Organic], 21),    // Jaja XL wolny wybieg
            (149, 200f, Unit.Grams,     30, "Masło",            StorageLocation.Fridge,  [], 8),                    // Piątnica
            (119, 1000f, Unit.Grams,   365, "Ryż basmati",      StorageLocation.Pantry,  [], 37),                   // Britta 1kg
            (20,  125f, Unit.Grams,      7, "Mozzarella",       StorageLocation.Fridge,  [], 45),                   // Galbani
            (108, 500f, Unit.Grams,      5, "Chleb razowy",     StorageLocation.Pantry,  [], 24),                   // Chleb razowy żytni
            (95,  1000f, Unit.Mililiter, 14, "Sok jabłkowy",    StorageLocation.Fridge,  [], 35),                   // Tymbark Jabłkowy
        ]);

        // ── Anna's kitchen — Dariusz (memberId 3) ──────────────────────
        SeedKitchen(session, KitchenAnna, memberId: 3, [
            (8,   500f, Unit.Grams,      4, "Mięso mielone",    StorageLocation.Freezer, [], null),
            (60,  300f, Unit.Grams,     14, "Cebula",           StorageLocation.Pantry,  [ItemTag.Vegan], null),
            (98,  500f, Unit.Mililiter,  90, "Piwo Żywiec",     StorageLocation.Fridge,  [ItemTag.ForParty], 27),   // Żywiec
            (98,  500f, Unit.Mililiter,  90, "Piwo Tyskie",     StorageLocation.Fridge,  [ItemTag.ForParty], 28),   // Tyskie
            (14,  200f, Unit.Grams,     14,  "Kabanosy",        StorageLocation.Fridge,  [], 18),                   // Kabanosy 200g
            (26,  100f, Unit.Grams,     60,  "Parmezan",        StorageLocation.Fridge,  [], 47),                   // Parmezan tarty
        ]);

        // ── Dariusz's kitchen (memberId 3) ──────────────────────────────
        SeedKitchen(session, KitchenDariusz, memberId: 3, [
            (18,  300f, Unit.Grams,      7, "Boczek",           StorageLocation.Fridge,  [ItemTag.HighProtein], null),
            (14,  500f, Unit.Grams,     10, "Kiełbasa",         StorageLocation.Fridge,  [], 17),                   // Kiełbasa Śląska
            (20,  150f, Unit.Grams,     14, "Mozzarella",       StorageLocation.Fridge,  [], 46),                   // Zott
            (41,  200f, Unit.Grams,      5, "Szpinak",          StorageLocation.Fridge,  [ItemTag.Vegan, ItemTag.GlutenFree], null),
            (7,   800f, Unit.Grams,      4, "Karkówka na grilla", StorageLocation.Freezer, [ItemTag.ForParty], null),
            (98,  500f, Unit.Mililiter, 90, "Piwo Lech",         StorageLocation.Fridge,  [ItemTag.ForParty], 29),  // Lech Premium
            (98,  500f, Unit.Mililiter, 90, "Piwo Książęce",     StorageLocation.Fridge,  [], 30),                  // Książęce Złote
            (22,  1000f, Unit.Mililiter, 5, "Mleko UHT",         StorageLocation.Fridge,  [], 3),                   // Łaciate UHT
            (2,   150f, Unit.Grams,      7, "Polędwica",         StorageLocation.Fridge,  [], 16),                  // Polędwica Sopocka
            (90,  500f, Unit.Grams,    180, "Kawa Jacobs",       StorageLocation.Pantry,  [], 32),                  // Jacobs Krönung
            (121, 500f, Unit.Grams,    365, "Makaron spaghetti", StorageLocation.Pantry,  [], 39),                  // Lubella
            (134, 200f, Unit.Grams,      5, "Łosoś wędzony",    StorageLocation.Fridge,  [ItemTag.HighProtein], 42), // Łosoś wędzony
            (27,  200f, Unit.Grams,     21, "Ser Edam",          StorageLocation.Fridge,  [], 14),                  // Edam
            (78,  500f, Unit.Grams,      7, "Jabłka",            StorageLocation.Fridge,  [ItemTag.Vegan], null),
            (79,  300f, Unit.Grams,      5, "Banany",            StorageLocation.Pantry,  [ItemTag.Vegan], null),
        ]);

        await session.SaveChangesAsync(ct);
        Console.WriteLine("[DevInventorySeeder] Seeded inventory events for 3 kitchens.");
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    private static void SeedKitchen(
        IDocumentSession session,
        Guid kitchenId,
        int memberId,
        List<(short foodProductId, float amount, Unit unit, int daysUntilExpiry, string note, StorageLocation location, List<ItemTag> tags, int? variantId)> items)
    {
        var events = new List<object>();
        foreach (var (foodProductId, amount, unit, daysUntilExpiry, note, location, tags, variantId) in items)
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
                DateTimeOffset.UtcNow,
                variantId));
        }

        session.Events.Append(kitchenId, events.ToArray());
    }
}
