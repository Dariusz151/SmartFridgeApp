using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class RemoveFridgeItemRequest
    {
        public long FridgeItemId { get; set; }
        public Guid UserId { get; set; }
    }
}
