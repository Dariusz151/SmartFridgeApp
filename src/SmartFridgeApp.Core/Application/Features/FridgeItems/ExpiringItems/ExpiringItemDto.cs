using System;

namespace SmartFridgeApp.Core.Application.Features;

public class ExpiringItemDto
{
    public long FridgeItemId { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public float Value { get; set; }
    public string Unit { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int DaysUntilExpiry { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
}
