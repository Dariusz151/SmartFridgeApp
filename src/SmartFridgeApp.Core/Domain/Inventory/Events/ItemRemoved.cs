using System;

namespace SmartFridgeApp.Core.Domain.Inventory.Events;

public record ItemRemoved(
    Guid ItemId,
    int MemberId,
    DateTimeOffset RemovedAt);
