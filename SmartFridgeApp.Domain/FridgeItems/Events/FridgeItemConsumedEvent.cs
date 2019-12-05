using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.FridgeItems.Events
{
    public class FridgeItemConsumedEvent : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemConsumedEvent(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
