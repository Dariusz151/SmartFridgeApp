using System;

namespace SmartFridgeApp.Core.Domain.ShoppingList.Events;

public record ItemBought(
    Guid ItemId,
    string BoughtByEmail,
    DateTimeOffset BoughtAt);
