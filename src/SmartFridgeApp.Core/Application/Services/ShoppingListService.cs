using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features.ShoppingList;
using SmartFridgeApp.Core.Contracts.Repositories;

namespace SmartFridgeApp.Core.Application.Services;

public class ShoppingListService(IShoppingListRepository shoppingListRepository) : IShoppingListService
{
    public async Task<IReadOnlyList<ShoppingListItemDto>> GetItemsAsync(Guid kitchenId, CancellationToken ct = default) =>
        await shoppingListRepository.GetItemsByKitchenAsync(kitchenId, ct);

    public async Task<ShoppingListItemDto> AddItemAsync(Guid kitchenId, string name, string addedByEmail, CancellationToken ct = default)
    {
        var (aggregate, version) = await shoppingListRepository.LoadAsync(kitchenId, ct);
        var evt = aggregate.AddItem(name, addedByEmail);
        await shoppingListRepository.AppendEventsAsync(kitchenId, version, [evt], ct);

        return new ShoppingListItemDto
        {
            Id = evt.ItemId,
            KitchenId = kitchenId,
            Name = evt.Name,
            AddedByEmail = evt.AddedByEmail,
            AddedAt = evt.AddedAt
        };
    }

    public async Task BuyItemAsync(Guid kitchenId, Guid itemId, string boughtByEmail, CancellationToken ct = default)
    {
        var (aggregate, version) = await shoppingListRepository.LoadAsync(kitchenId, ct);
        var evt = aggregate.BuyItem(itemId, boughtByEmail);
        await shoppingListRepository.AppendEventsAsync(kitchenId, version, [evt], ct);
    }

    public async Task RemoveItemAsync(Guid kitchenId, Guid itemId, string removedByEmail, CancellationToken ct = default)
    {
        var (aggregate, version) = await shoppingListRepository.LoadAsync(kitchenId, ct);
        var evt = aggregate.RemoveItem(itemId, removedByEmail);
        await shoppingListRepository.AppendEventsAsync(kitchenId, version, [evt], ct);
    }
}
