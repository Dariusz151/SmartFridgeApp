using System;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Domain.Events;

public class FridgeItemAddedDomainEvent : DomainEventBase
{
    public Guid FridgeId { get; }
    public long FridgeItemId { get; }
    public int MemberId { get; }

    public FridgeItemAddedDomainEvent(Guid fridgeId, long fridgeItemId, int memberId)
    {
        FridgeId = fridgeId;
        FridgeItemId = fridgeItemId;
        MemberId = memberId;
    }
}
