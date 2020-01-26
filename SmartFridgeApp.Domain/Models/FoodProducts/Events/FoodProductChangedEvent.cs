using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.FoodProducts.Events
{
    public class FoodProductChangedEvent : DomainEventBase
    {
        public FoodProduct FoodProduct { get; }

        public FoodProductChangedEvent(FoodProduct foodProduct)
        {
            FoodProduct = foodProduct;
        }
    }
}
