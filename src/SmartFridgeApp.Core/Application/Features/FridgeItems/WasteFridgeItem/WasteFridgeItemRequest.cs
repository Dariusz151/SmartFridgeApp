using System;

namespace SmartFridgeApp.Core.Application.Features;

public class WasteFridgeItemRequest
{
    public long FridgeItemId { get; set; }
    public Guid UserId { get; set; }
    public string Reason { get; set; }
}
