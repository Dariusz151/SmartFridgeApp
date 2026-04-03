using System;
using System.Collections.Generic;
using SmartFridgeApp.Core.Domain.ShoppingList.Events;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Core.Domain.ShoppingList;

public class KitchenShoppingList
{
    public Guid Id { get; set; }

    private readonly Dictionary<Guid, ShoppingItem> _items = new();
    public IReadOnlyDictionary<Guid, ShoppingItem> Items => _items;

    public int TotalItemsAdded { get; private set; }
    public int TotalItemsBought { get; private set; }

    public ItemAddedToShoppingList AddItem(string name, string addedByEmail)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidInputException("Shopping list item must have a name.", "InvalidShoppingItemName");

        var evt = new ItemAddedToShoppingList(Guid.NewGuid(), name.Trim(), addedByEmail, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    public ItemBought BuyItem(Guid itemId, string boughtByEmail)
    {
        if (!_items.ContainsKey(itemId))
            throw new DomainException("Item not found in shopping list.", "ShoppingItemNotFound");

        var evt = new ItemBought(itemId, boughtByEmail, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    public ItemRemovedFromShoppingList RemoveItem(Guid itemId, string removedByEmail)
    {
        if (!_items.ContainsKey(itemId))
            throw new DomainException("Item not found in shopping list.", "ShoppingItemNotFound");

        var evt = new ItemRemovedFromShoppingList(itemId, removedByEmail, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    public void Apply(ItemAddedToShoppingList e)
    {
        _items[e.ItemId] = new ShoppingItem(e.ItemId, e.Name, e.AddedByEmail, e.AddedAt);
        TotalItemsAdded++;
    }

    public void Apply(ItemBought e)
    {
        _items.Remove(e.ItemId);
        TotalItemsBought++;
    }

    public void Apply(ItemRemovedFromShoppingList e)
    {
        _items.Remove(e.ItemId);
    }
}
