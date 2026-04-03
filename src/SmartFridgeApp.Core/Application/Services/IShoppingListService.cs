using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features.ShoppingList;

namespace SmartFridgeApp.Core.Application.Services;

public interface IShoppingListService
{
    Task<IReadOnlyList<ShoppingListItemDto>> GetItemsAsync(Guid kitchenId, CancellationToken ct = default);
    Task<ShoppingListItemDto> AddItemAsync(Guid kitchenId, string name, string addedByEmail, CancellationToken ct = default);
    Task BuyItemAsync(Guid kitchenId, Guid itemId, string boughtByEmail, CancellationToken ct = default);
    Task RemoveItemAsync(Guid kitchenId, Guid itemId, string removedByEmail, CancellationToken ct = default);
}
