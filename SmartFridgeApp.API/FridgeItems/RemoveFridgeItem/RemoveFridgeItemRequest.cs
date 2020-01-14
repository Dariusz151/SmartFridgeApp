using System;

namespace SmartFridgeApp.API.FridgeItems.RemoveFridgeItem
{
    public class RemoveFridgeItemRequest
    {
        public Guid FridgeItemId { get; set; }
        public Guid UserId { get; set; }
    }
}
