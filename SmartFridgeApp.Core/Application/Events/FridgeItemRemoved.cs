using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Events
{
    public class FridgeItemRemoved : DomainEventBase
    {
        public FridgeItem FridgeItem { get; }

        public FridgeItemRemoved(FridgeItem fridgeItem)
        {
            FridgeItem = fridgeItem;
        }
    }
}
