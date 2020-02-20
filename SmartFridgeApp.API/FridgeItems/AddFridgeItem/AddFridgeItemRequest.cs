using System;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemRequest
    {
        public AddFridgeItemDto FridgeItem { get; set; }
        public int UserId { get; set; }
    }
}
