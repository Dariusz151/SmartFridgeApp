using System;
using System.Collections.Generic;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Infrastructure.Inventory;

public class ActiveStockItemDocument
{
    public Guid Id { get; set; }
    public Guid KitchenId { get; set; }
    public short FoodProductId { get; set; }
    public int MemberId { get; set; }
    public float Amount { get; set; }
    public Unit Unit { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public string Note { get; set; }
    public StorageLocation Location { get; set; }
    public List<ItemTag> Tags { get; set; } = [];
    public DateTimeOffset StockedAt { get; set; }
    public int? VariantId { get; set; }
}
