using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems
{
    public class FridgeItemDto
    {
        public string ProductName { get; set; }
        public int FoodProductId { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public float Value { get; set; }
        public Unit Unit { get; set; }
    }
}
