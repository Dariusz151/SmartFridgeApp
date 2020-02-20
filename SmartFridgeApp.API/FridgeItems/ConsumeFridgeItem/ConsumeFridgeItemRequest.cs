using System;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems.ConsumeFridgeItem
{
    public class ConsumeFridgeItemRequest
    {
        public long FridgeItemId { get; set; }
        public int UserId { get; set; }
        public AmountValue AmountValue {get; set; }
    }
}
