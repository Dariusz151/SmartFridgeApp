using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Users.Events
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
