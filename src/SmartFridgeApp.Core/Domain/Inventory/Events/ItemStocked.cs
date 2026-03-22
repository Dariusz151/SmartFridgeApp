using System;
using System.Collections.Generic;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Domain.Inventory.Events;

public record ItemStocked(
    Guid ItemId,
    short FoodProductId,
    int MemberId,
    float Amount,
    Unit Unit,
    DateTimeOffset ExpirationDate,
    string Note,
    StorageLocation Location,
    List<ItemTag> Tags,
    DateTimeOffset StockedAt);
