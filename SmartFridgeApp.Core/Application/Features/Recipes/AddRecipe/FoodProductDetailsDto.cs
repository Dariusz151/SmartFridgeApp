

using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Features
{
    public class FoodProductDetailsDto
    {
        public short FoodProductId { get; set; }
        public bool IsOptional { get; set; }
        public AmountValue AmountValue { get; set; }
    }
}
