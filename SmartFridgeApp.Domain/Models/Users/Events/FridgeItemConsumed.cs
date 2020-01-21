using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Users.Events
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
