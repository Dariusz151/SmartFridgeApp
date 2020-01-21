using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.FoodProducts.Events
{
    public class FoodProductAddedEvent : DomainEventBase
    {
        public FoodProduct FoodProduct { get; }
        public FoodProductAddedEvent(FoodProduct foodProduct)
        {
            FoodProduct = foodProduct;
        }
    }
}
