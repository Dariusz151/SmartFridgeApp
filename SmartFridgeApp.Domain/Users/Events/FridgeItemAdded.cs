using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Users.Events
{
    public class FridgeItemAdded : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemAdded(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
