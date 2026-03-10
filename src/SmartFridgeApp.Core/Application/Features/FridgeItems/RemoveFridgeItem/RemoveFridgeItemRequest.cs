namespace SmartFridgeApp.Core.Application.Features
{
    public class RemoveFridgeItemRequest
    {
        public long FridgeItemId { get; set; }
        public int MemberId { get; set; }
    }
}
