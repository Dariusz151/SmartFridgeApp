using System;

namespace SmartFridgeApp.Shared.Domain;

public class DomainEventBase : IDomainEvent
{
    public DomainEventBase()
    {
        OccurredOn = DateTime.Now;
    }

    public DateTime OccurredOn { get; }
}
