using System;

namespace SmartFridgeApp.Core.SeedWork
{
    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            this.OccurredOn = DateTime.Now;
        }
        
        public DateTime OccurredOn { get; }
    }
}
