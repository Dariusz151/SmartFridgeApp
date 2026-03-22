using System;

namespace SmartFridgeApp.Core.Domain.Inventory.Events;

public record ItemExpired(
    Guid ItemId,
    DateTimeOffset ExpiredAt);
