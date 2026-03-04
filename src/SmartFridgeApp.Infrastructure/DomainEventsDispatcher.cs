using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SmartFridgeApp.Infrastructure.Notifications;
using SmartFridgeApp.Infrastructure.SeedWork;
using SmartFridgeApp.Shared.Domain;
using SmartFridgeApp.Shared.Outbox;
using SmartFridgeApp.Core.Application.Events;

namespace SmartFridgeApp.Infrastructure
{
    public class DomainEventsDispatcher(SmartFridgeAppContext context) : IDomainEventsDispatcher
    {
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.All
        };

        public async Task DispatchEventsAsync()
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                var notification = CreateNotification(domainEvent);
                if (notification == null) continue;

                var data = JsonConvert.SerializeObject(notification, JsonSettings);
                var outboxMessage = new OutboxMessage(
                    domainEvent.OccurredOn,
                    notification.GetType().FullName!,
                    data);

                await context.OutboxMessages.AddAsync(outboxMessage);
            }
        }

        private static object? CreateNotification(IDomainEvent domainEvent) => domainEvent switch
        {
            FridgeCreatedEvent e => new FridgeAddedNotification(e),
            RecipeAddedEvent e => new RecipeAddedNotification(e),
            UserAddedEvent e => new UserAddedNotification(e),
            _ => null
        };
    }
}
