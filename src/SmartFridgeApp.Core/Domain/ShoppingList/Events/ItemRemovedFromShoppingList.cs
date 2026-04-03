using System;

namespace SmartFridgeApp.Core.Domain.ShoppingList.Events;

public record ItemRemovedFromShoppingList(
    Guid ItemId,
    string RemovedByEmail,
    DateTimeOffset RemovedAt);
