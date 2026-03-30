using System;

namespace SmartFridgeApp.Core.Domain.Inventory.Events;

public record ItemConsumed(
    Guid ItemId,
    int MemberId,
    float AmountConsumed,
    bool IsFullyConsumed,
    DateTimeOffset ConsumedAt);
