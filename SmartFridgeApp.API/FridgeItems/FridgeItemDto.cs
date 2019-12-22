using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems
{
    public class FridgeItemDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public Category Category { get; set; }
        public float Value { get; set; }
        public Unit Unit { get; set; }
    }
}
