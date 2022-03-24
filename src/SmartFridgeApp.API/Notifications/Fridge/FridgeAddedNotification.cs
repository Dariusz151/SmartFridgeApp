using System;
using Newtonsoft.Json;
using SmartFridgeApp.Infrastructure.SeedWork;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Application.Events;

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
