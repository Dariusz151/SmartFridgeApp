using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Models.Fridges.Events
{
    public class FridgeUpdatedEvent : DomainEventBase
    {
        public Fridge Fridge { get; }
        public FridgeUpdatedEvent(Fridge fridge)
        {
            Fridge = fridge;
        }
    }
}
