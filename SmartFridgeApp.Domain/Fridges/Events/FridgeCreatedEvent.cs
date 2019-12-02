using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Fridges.Events
{
    public class FridgeCreatedEvent : DomainEventBase
    {
        public Fridge Fridge { get; }
        public FridgeCreatedEvent(Fridge fridge)
        {
            Fridge = fridge;
        }
    }
}
