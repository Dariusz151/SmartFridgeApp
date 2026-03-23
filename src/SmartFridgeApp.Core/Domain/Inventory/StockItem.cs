using System;
using System.Collections.Generic;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Domain.Inventory;

public class StockItem
{
    public Guid Id { get; private set; }
    public short FoodProductId { get; private set; }
    public int MemberId { get; private set; }
    public float Amount { get; private set; }
    public Unit Unit { get; private set; }
    public DateTimeOffset ExpirationDate { get; private set; }
    public string Note { get; private set; }
    public StorageLocation Location { get; private set; }
    public List<ItemTag> Tags { get; private set; }
    public DateTimeOffset StockedAt { get; private set; }
    public int? VariantId { get; private set; }

    internal StockItem(Guid id, short foodProductId, int memberId, float amount, Unit unit, DateTimeOffset expirationDate, string note, StorageLocation location, List<ItemTag> tags, DateTimeOffset stockedAt, int? variantId = null)
    {
        Id = id;
        FoodProductId = foodProductId;
        MemberId = memberId;
        Amount = amount;
        Unit = unit;
        ExpirationDate = expirationDate;
        Note = note;
        Location = location;
        Tags = tags ?? [];
        StockedAt = stockedAt;
        VariantId = variantId;
    }

    internal void DecreaseAmount(float consumed) => Amount -= consumed;

    internal void IncreaseAmount(float added, DateTimeOffset newExpiration)
    {
        Amount += added;
        if (newExpiration > ExpirationDate)
            ExpirationDate = newExpiration;
    }

    public bool IsExpired() => ExpirationDate < DateTimeOffset.UtcNow;
}
