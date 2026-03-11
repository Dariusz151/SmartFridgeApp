using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class ConsumeFridgeItemRequest
    {
        public long FridgeItemId { get; set; }
        public int MemberId { get; set; }
        public AmountValue AmountValue { get; set; }
    }
}
