using Newtonsoft.Json;
using SmartFridgeApp.Domain.Models.Fridges.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Fridges.IntegrationHandlers
{
    public class FridgeAddedNotification : DomainNotificationBase<FridgeCreatedEvent>
    {
        public int FridgeId { get; }

        public FridgeAddedNotification(FridgeCreatedEvent domainEvent) : base(domainEvent)
        {
            this.FridgeId = domainEvent.Fridge.Id;
        }

        [JsonConstructor]
        public FridgeAddedNotification(int fridgeId) : base(null)
        {
            this.FridgeId = fridgeId;
        }
    }
}
