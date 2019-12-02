using MediatR;
using System;

namespace SmartFridgeApp.Domain.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
