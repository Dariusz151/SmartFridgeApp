using System;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Domain.Events;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class ShoppingNeededNotification : DomainNotificationBase<ShoppingNeededDomainEvent>
{
    public Guid FridgeId { get; }
    public int ActiveItemCount { get; }
    public double AverageItemCount { get; }

    public ShoppingNeededNotification(ShoppingNeededDomainEvent domainEvent)
        : base(domainEvent)
    {
        FridgeId = domainEvent.FridgeId;
        ActiveItemCount = domainEvent.ActiveItemCount;
        AverageItemCount = domainEvent.AverageItemCount;
    }

    [JsonConstructor]
    public ShoppingNeededNotification(Guid fridgeId, int activeItemCount, double averageItemCount)
        : base(null)
    {
        FridgeId = fridgeId;
        ActiveItemCount = activeItemCount;
        AverageItemCount = averageItemCount;
    }
}
