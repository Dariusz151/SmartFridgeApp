using SmartFridgeApp.Core.Domain.Shared;
using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class ConsumeFridgeItemRequest
    {
        public long FridgeItemId { get; set; }
        public Guid UserId { get; set; }
        public AmountValue AmountValue {get; set; }
    }
}
