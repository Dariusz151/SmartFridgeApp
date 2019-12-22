using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Users.Events
{
    public class FridgeItemRemoved :DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemRemoved(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
