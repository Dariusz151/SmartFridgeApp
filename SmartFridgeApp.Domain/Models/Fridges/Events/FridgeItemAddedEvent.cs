using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Fridges.Events
{
    public class FridgeItemAddedEvent : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemAddedEvent(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
