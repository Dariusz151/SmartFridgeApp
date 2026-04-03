using System;

namespace SmartFridgeApp.Core.Domain.ShoppingList.Events;

public record ItemAddedToShoppingList(
    Guid ItemId,
    string Name,
    string AddedByEmail,
    DateTimeOffset AddedAt);
