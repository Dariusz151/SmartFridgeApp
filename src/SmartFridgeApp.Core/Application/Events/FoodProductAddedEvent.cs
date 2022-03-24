using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Application.Events
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
