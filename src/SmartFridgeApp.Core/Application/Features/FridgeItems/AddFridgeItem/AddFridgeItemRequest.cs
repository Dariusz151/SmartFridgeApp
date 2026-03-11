using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class AddFridgeItemRequest
    {
        public AddFridgeItemDto FridgeItem { get; set; }
        public int MemberId { get; set; }
    }
}
