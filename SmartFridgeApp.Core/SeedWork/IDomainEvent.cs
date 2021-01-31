using MediatR;
using System;

namespace SmartFridgeApp.Core.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
