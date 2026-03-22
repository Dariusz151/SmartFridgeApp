using System;

namespace SmartFridgeApp.Core.Application.Features;

public class WasteItemRequest
{
    public Guid StockItemId { get; set; }
    public int MemberId { get; set; }
    public string Reason { get; set; }
}
