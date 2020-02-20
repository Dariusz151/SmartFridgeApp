using System;

namespace SmartFridgeApp.API.FridgeItems.RemoveFridgeItem
{
    public class RemoveFridgeItemRequest
    {
        public long FridgeItemId { get; set; }
        public int UserId { get; set; }
    }
}
