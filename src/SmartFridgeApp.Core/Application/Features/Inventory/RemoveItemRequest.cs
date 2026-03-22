using System;

namespace SmartFridgeApp.Core.Application.Features;

public class RemoveItemRequest
{
    public Guid StockItemId { get; set; }
    public int MemberId { get; set; }
}
