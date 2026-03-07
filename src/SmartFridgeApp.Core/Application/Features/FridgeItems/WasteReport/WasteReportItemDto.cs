using System;

namespace SmartFridgeApp.Core.Application.Features;

public class WasteReportItemDto
{
    public long FridgeItemId { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public float Amount { get; set; }
    public string Unit { get; set; }
    public DateTime WastedAt { get; set; }
    public string WasteReason { get; set; }
}
