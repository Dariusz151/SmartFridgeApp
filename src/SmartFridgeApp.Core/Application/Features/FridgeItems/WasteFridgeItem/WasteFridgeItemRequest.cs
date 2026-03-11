namespace SmartFridgeApp.Core.Application.Features;

public class WasteFridgeItemRequest
{
    public long FridgeItemId { get; set; }
    public int MemberId { get; set; }
    public string Reason { get; set; }
}
