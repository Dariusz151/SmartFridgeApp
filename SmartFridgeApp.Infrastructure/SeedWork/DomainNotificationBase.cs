using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Newtonsoft.Json;

namespace SmartFridgeApp.Infrastructure.SeedWork
{
    public class DomainNotificationBase<T> : IDomainEventNotification<T>, INotification
    {
        [JsonIgnore]
        public T DomainEvent { get; }

        public DomainNotificationBase(T domainEvent)
        {
            this.DomainEvent = domainEvent;
        }
    }
}
