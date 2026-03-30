using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Application.Events
{
    public class KitchenCreatedEvent : DomainEventBase
    {
        public Kitchen Kitchen { get; }
        public KitchenCreatedEvent(Kitchen kitchen)
        {
            Kitchen = kitchen;
        }
    }
}
