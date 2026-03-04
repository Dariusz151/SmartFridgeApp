using System;

namespace SmartFridgeApp.Shared.Domain
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
