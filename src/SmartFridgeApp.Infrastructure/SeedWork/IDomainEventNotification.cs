namespace SmartFridgeApp.Infrastructure.SeedWork
{
    public interface IDomainEventNotification<out TEventType>
    {
        TEventType DomainEvent { get; }
    }
}
