using System;

namespace SmartFridgeApp.Core.Domain.Inventory.Events;

public record ItemWasted(
    Guid ItemId,
    int MemberId,
    string Reason,
    DateTimeOffset WastedAt);
