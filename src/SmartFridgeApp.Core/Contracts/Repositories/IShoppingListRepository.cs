using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features.ShoppingList;
using SmartFridgeApp.Core.Domain.ShoppingList;

namespace SmartFridgeApp.Core.Contracts.Repositories;

public interface IShoppingListRepository
{
    Task<(KitchenShoppingList Aggregate, long Version)> LoadAsync(Guid kitchenId, CancellationToken ct = default);
    Task AppendEventsAsync(Guid kitchenId, long expectedVersion, IReadOnlyList<object> events, CancellationToken ct = default);
    Task<IReadOnlyList<ShoppingListItemDto>> GetItemsByKitchenAsync(Guid kitchenId, CancellationToken ct = default);
}
