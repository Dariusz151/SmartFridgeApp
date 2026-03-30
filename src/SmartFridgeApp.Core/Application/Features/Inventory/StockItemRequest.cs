using System;
using System.Collections.Generic;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features;

public class StockItemRequest
{
    public short FoodProductId { get; set; }
    public float Amount { get; set; }
    public Unit Unit { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public string Note { get; set; }
    public StorageLocation Location { get; set; } = StorageLocation.Fridge;
    public List<ItemTag> Tags { get; set; } = [];
    public int? VariantId { get; set; }
}
