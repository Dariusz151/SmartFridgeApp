using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Domain.Events;

public class FridgeItemConsumedDomainEvent : DomainEventBase
{
    public long FridgeItemId { get; }
    public int MemberId { get; }

    public FridgeItemConsumedDomainEvent(long fridgeItemId, int memberId)
    {
        FridgeItemId = fridgeItemId;
        MemberId = memberId;
    }
}
