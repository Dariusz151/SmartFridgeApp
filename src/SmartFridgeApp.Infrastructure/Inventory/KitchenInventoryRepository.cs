using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Inventory;
using SmartFridgeApp.Core.Domain.Inventory.Events;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Infrastructure.Inventory;

public class KitchenInventoryRepository(IDocumentSession session) : IKitchenInventoryRepository
{
    public async Task<(KitchenInventory Aggregate, long Version)> LoadAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var state = await session.Events.FetchStreamStateAsync(kitchenId, ct);
        if (state is null)
            return (new KitchenInventory { Id = kitchenId }, 0);

        var aggregate = await session.Events.AggregateStreamAsync<KitchenInventory>(kitchenId, token: ct);
        return (aggregate ?? new KitchenInventory { Id = kitchenId }, state.Version);
    }

    public async Task AppendEventsAsync(Guid kitchenId, long expectedVersion, IReadOnlyList<object> events, CancellationToken ct = default)
    {
        session.Events.Append(kitchenId, events.ToArray());
        await session.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByKitchenAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var docs = await session.Query<ActiveStockItemDocument>()
            .Where(d => d.KitchenId == kitchenId)
            .ToListAsync(ct);

        return docs.Select(ToStockItemDto).ToList();
    }

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByMemberAsync(Guid kitchenId, int memberId, CancellationToken ct = default)
    {
        var docs = await session.Query<ActiveStockItemDocument>()
            .Where(d => d.KitchenId == kitchenId && d.MemberId == memberId)
            .ToListAsync(ct);

        return docs.Select(ToStockItemDto).ToList();
    }

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByLocationAsync(Guid kitchenId, StorageLocation location, CancellationToken ct = default)
    {
        var docs = await session.Query<ActiveStockItemDocument>()
            .Where(d => d.KitchenId == kitchenId && d.Location == location)
            .ToListAsync(ct);

        return docs.Select(ToStockItemDto).ToList();
    }

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByTagAsync(Guid kitchenId, ItemTag tag, CancellationToken ct = default)
    {
        var docs = await session.Query<ActiveStockItemDocument>()
            .Where(d => d.KitchenId == kitchenId && d.Tags.Contains(tag))
            .ToListAsync(ct);

        return docs.Select(ToStockItemDto).ToList();
    }

    public async Task<IReadOnlyList<ExpiringItemDto>> GetExpiringItemsAsync(Guid kitchenId, int daysThreshold, CancellationToken ct = default)
    {
        var threshold = DateTimeOffset.UtcNow.AddDays(daysThreshold);

        var docs = await session.Query<ActiveStockItemDocument>()
            .Where(d => d.KitchenId == kitchenId && d.ExpirationDate <= threshold)
            .OrderBy(d => d.ExpirationDate)
            .ToListAsync(ct);

        return docs.Select(d => new ExpiringItemDto
        {
            StockItemId = d.Id,
            FoodProductId = d.FoodProductId,
            Amount = d.Amount,
            Unit = d.Unit.ToString(),
            ExpirationDate = d.ExpirationDate,
            DaysUntilExpiry = (int)(d.ExpirationDate - DateTimeOffset.UtcNow).TotalDays,
            MemberId = d.MemberId,
            Location = d.Location.ToString()
        }).ToList();
    }

    public async Task<IReadOnlyList<WasteReportItemDto>> GetWastedItemsAsync(Guid kitchenId, int year, int month, CancellationToken ct = default)
    {
        var events = await session.Events.QueryRawEventDataOnly<ItemWasted>()
            .Where(e => e.WastedAt.Year == year && e.WastedAt.Month == month)
            .ToListAsync(ct);

        var streamEvents = await session.Events.FetchStreamAsync(kitchenId, token: ct);
        var kitchenWastedIds = streamEvents
            .Where(e => e.Data is ItemWasted)
            .Select(e => ((ItemWasted)e.Data).ItemId)
            .ToHashSet();

        return events
            .Where(e => kitchenWastedIds.Contains(e.ItemId))
            .Select(e => new WasteReportItemDto
            {
                StockItemId = e.ItemId,
                Amount = 0,
                Unit = string.Empty,
                WastedAt = e.WastedAt,
                WasteReason = e.Reason
            })
            .OrderByDescending(e => e.WastedAt)
            .ToList();
    }

    private static StockItemDto ToStockItemDto(ActiveStockItemDocument d) => new()
    {
        StockItemId = d.Id,
        FoodProductId = d.FoodProductId,
        MemberId = d.MemberId,
        Amount = d.Amount,
        Unit = d.Unit.ToString(),
        ExpirationDate = d.ExpirationDate,
        Note = d.Note,
        Location = d.Location.ToString(),
        Tags = d.Tags?.Select(t => t.ToString()).ToList() ?? [],
        StockedAt = d.StockedAt,
        VariantId = d.VariantId
    };
}
