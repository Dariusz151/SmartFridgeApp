using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartFridgeApp.Shared.Domain;

public abstract class Entity
{
    [JsonIgnore]
    private List<IDomainEvent> _domainEvents;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= [];
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
