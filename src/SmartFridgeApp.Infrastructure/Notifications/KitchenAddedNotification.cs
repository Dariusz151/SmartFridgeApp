using System;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class KitchenAddedNotification : DomainNotificationBase<KitchenCreatedEvent>
{
    public Guid kitchenId { get; }

    public KitchenAddedNotification(KitchenCreatedEvent domainEvent) : base(domainEvent)
    {
        kitchenId = domainEvent.Kitchen.Id;
    }

    [JsonConstructor]
    public KitchenAddedNotification(Guid kitchenId) : base(null)
    {
        kitchenId = kitchenId;
    }
}
