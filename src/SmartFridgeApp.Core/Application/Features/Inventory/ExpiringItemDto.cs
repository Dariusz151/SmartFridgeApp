using System;

namespace SmartFridgeApp.Core.Application.Features;

public class ExpiringItemDto
{
    public Guid StockItemId { get; set; }
    public short FoodProductId { get; set; }
    public float Amount { get; set; }
    public string Unit { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public int DaysUntilExpiry { get; set; }
    public int MemberId { get; set; }
    public string Location { get; set; }
}
