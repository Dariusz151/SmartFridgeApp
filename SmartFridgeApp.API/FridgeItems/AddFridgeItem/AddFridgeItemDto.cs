using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemDto
    {
        public int FoodProductId { get; set; }
        public float Value { get; set; }
        public string Note { get; set; }
        public Unit Unit { get; set; }
    }
}
