using System;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features;

public class ConsumeItemRequest
{
    public Guid StockItemId { get; set; }
    public int MemberId { get; set; }
    public float Amount { get; set; }
    public Unit Unit { get; set; }
}
