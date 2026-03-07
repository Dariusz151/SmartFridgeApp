using System;

namespace SmartFridgeApp.Shared.Domain;

public class DomainEventBase : IDomainEvent
{
    public DomainEventBase()
    {
        OccurredOn = DateTime.UtcNow;
    }

    public DateTime OccurredOn { get; }
}
