

using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class AddFridgeItemDto
    {
        public int FoodProductId { get; set; }
        public float Value { get; set; }
        public string Note { get; set; }
        public Unit Unit { get; set; }
    }
}
