using Newtonsoft.Json;

namespace SmartFridgeApp.Infrastructure.SeedWork
{
    public class DomainNotificationBase<T> : IDomainEventNotification<T>
    {
        [JsonIgnore]
        public T DomainEvent { get; }

        public DomainNotificationBase(T domainEvent)
        {
            this.DomainEvent = domainEvent;
        }
    }
}
