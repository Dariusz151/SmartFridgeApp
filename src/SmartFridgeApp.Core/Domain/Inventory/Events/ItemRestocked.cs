using System;

namespace SmartFridgeApp.Core.Domain.Inventory.Events;

public record ItemRestocked(
    Guid ItemId,
    float AddedAmount,
    DateTimeOffset NewExpirationDate,
    DateTimeOffset RestockedAt);
