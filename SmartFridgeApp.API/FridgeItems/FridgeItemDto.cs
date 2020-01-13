using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems
{
    public class FridgeItemDto
    {
        public int FoodProductId { get; set; }
        public string Desc { get; set; }
        public Category Category { get; set; }
        public float Value { get; set; }
        public Unit Unit { get; set; }
    }
}
