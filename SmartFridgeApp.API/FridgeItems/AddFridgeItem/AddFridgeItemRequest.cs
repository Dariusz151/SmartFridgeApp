using System;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemRequest
    {
        public FridgeItemDto FridgeItem { get; set; }
        public Guid UserId { get; set; }
    }
}
