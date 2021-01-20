using System;
using Newtonsoft.Json;
using SmartFridgeApp.Domain.Models.Fridges.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Notifications.Fridge
{
    public class FridgeAddedNotification : DomainNotificationBase<FridgeCreatedEvent>
    {
        public Guid FridgeId { get; }

        public FridgeAddedNotification(FridgeCreatedEvent domainEvent) : base(domainEvent)
        {
            this.FridgeId = domainEvent.Fridge.Id;
        }

        [JsonConstructor]
        public FridgeAddedNotification(Guid fridgeId) : base(null)
        {
            this.FridgeId = fridgeId;
        }
    }
}
