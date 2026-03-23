using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features;

public class StockItemDto
{
    public Guid StockItemId { get; set; }
    public short FoodProductId { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public int MemberId { get; set; }
    public float Amount { get; set; }
    public string Unit { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public string Note { get; set; }
    public string Location { get; set; }
    public List<string> Tags { get; set; } = [];
    public DateTimeOffset StockedAt { get; set; }
    public int? VariantId { get; set; }
    public string VariantName { get; set; }
}
