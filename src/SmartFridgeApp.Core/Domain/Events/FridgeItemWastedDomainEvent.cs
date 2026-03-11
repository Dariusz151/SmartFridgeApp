using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Domain.Events;

public class FridgeItemWastedDomainEvent : DomainEventBase
{
    public long FridgeItemId { get; }
    public int MemberId { get; }
    public string Reason { get; }

    public FridgeItemWastedDomainEvent(long fridgeItemId, int memberId, string reason)
    {
        FridgeItemId = fridgeItemId;
        MemberId = memberId;
        Reason = reason;
    }
}
