using System;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Domain.Events;

public class ShoppingNeededDomainEvent : DomainEventBase
{
    public Guid FridgeId { get; }
    public int ActiveItemCount { get; }
    public double AverageItemCount { get; }

    public ShoppingNeededDomainEvent(Guid fridgeId, int activeItemCount, double averageItemCount)
    {
        FridgeId = fridgeId;
        ActiveItemCount = activeItemCount;
        AverageItemCount = averageItemCount;
    }
}
