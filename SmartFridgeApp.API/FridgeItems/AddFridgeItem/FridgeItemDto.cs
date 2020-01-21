using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemDto
    {
        public int FoodProductId { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public float Value { get; set; }
        public Unit Unit { get; set; }
    }
}
