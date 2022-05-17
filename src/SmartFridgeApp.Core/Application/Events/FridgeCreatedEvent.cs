using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Application.Events
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
