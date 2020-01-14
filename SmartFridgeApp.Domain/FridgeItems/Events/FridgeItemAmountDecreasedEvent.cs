using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.FridgeItems.Events
{
    public class FridgeItemAmountDecreasedEvent : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemAmountDecreasedEvent(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
