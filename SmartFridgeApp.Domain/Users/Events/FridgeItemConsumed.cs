using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Users.Events
{
    public class FridgeItemConsumed : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemConsumed(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
