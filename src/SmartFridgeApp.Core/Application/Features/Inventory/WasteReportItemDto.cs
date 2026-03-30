using System;

namespace SmartFridgeApp.Core.Application.Features;

public class WasteReportItemDto
{
    public Guid StockItemId { get; set; }
    public float Amount { get; set; }
    public string Unit { get; set; }
    public DateTimeOffset WastedAt { get; set; }
    public string WasteReason { get; set; }
}
