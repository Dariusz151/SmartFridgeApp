using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Users.Events
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
