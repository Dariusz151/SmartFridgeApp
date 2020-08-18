using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartFridgeApp.Domain.Models.Fridges.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Fridges.IntegrationHandlers
{
    public class FridgeUpdatedNotification : DomainNotificationBase<FridgeUpdatedEvent>
    {
        public Guid FridgeId { get; }

        public FridgeUpdatedNotification(FridgeUpdatedEvent domainEvent) : base(domainEvent)
        {
            this.FridgeId = domainEvent.Fridge.Id;
        }

        [JsonConstructor]
        public FridgeUpdatedNotification(Guid fridgeId) : base(null)
        {
            this.FridgeId = fridgeId;
        }
    }
}
