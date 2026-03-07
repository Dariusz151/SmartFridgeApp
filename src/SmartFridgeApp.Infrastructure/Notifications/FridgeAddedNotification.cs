using System;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class FridgeAddedNotification : DomainNotificationBase<FridgeCreatedEvent>
{
    public Guid FridgeId { get; }

    public FridgeAddedNotification(FridgeCreatedEvent domainEvent) : base(domainEvent)
    {
        FridgeId = domainEvent.Fridge.Id;
    }

    [JsonConstructor]
    public FridgeAddedNotification(Guid fridgeId) : base(null)
    {
        FridgeId = fridgeId;
    }
}
