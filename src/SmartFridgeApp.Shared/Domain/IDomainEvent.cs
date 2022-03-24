using MediatR;
using System;

namespace SmartFridgeApp.Shared.Domain
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
