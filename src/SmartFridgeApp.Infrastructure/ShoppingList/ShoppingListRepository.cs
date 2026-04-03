using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using SmartFridgeApp.Core.Application.Features.ShoppingList;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.ShoppingList;

namespace SmartFridgeApp.Infrastructure.ShoppingList;

public class ShoppingListRepository(IDocumentSession session) : IShoppingListRepository
{
    public async Task<(KitchenShoppingList Aggregate, long Version)> LoadAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var state = await session.Events.FetchStreamStateAsync(kitchenId, ct);
        if (state is null)
            return (new KitchenShoppingList { Id = kitchenId }, 0);

        var aggregate = await session.Events.AggregateStreamAsync<KitchenShoppingList>(kitchenId, token: ct);
        return (aggregate ?? new KitchenShoppingList { Id = kitchenId }, state.Version);
    }

    public async Task AppendEventsAsync(Guid kitchenId, long expectedVersion, IReadOnlyList<object> events, CancellationToken ct = default)
    {
        session.Events.Append(kitchenId, events.ToArray());
        await session.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<ShoppingListItemDto>> GetItemsByKitchenAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var docs = await session.Query<ShoppingListItemDocument>()
            .Where(d => d.KitchenId == kitchenId)
            .OrderByDescending(d => d.AddedAt)
            .ToListAsync(ct);

        return docs.Select(d => new ShoppingListItemDto
        {
            Id = d.Id,
            KitchenId = d.KitchenId,
            Name = d.Name,
            AddedByEmail = d.AddedByEmail,
            AddedAt = d.AddedAt
        }).ToList();
    }
}
