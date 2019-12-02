using SmartFridgeApp.Domain.Fridges.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Fridges.Events
{
    public class FridgeItemAddedEvent : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemAddedEvent(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem
        }
    }
}
