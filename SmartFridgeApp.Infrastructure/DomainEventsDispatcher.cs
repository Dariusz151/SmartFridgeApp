using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using MediatR;
using Newtonsoft.Json;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Infrastructure.Outbox;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly SmartFridgeAppContext _context;

        public DomainEventsDispatcher(ILifetimeScope lifetimeScope, IMediator mediator, SmartFridgeAppContext context)
        {
            _lifetimeScope = lifetimeScope;
            _mediator = mediator;
            _context = context;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEntities = this._context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
            foreach (var domainEvent in domainEvents)
            {
                Type domainEventNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = _lifetimeScope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent)
                });

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as SeedWork.IDomainEventNotification<IDomainEvent>);
                }
            }

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            //var tasks = domainEvents
            //    .Select(async (domainEvent) =>
            //    {
            //        await _mediator.Publish(domainEvent);
            //    });

            //await Task.WhenAll(tasks);

            foreach (var domainEventNotification in domainEventNotifications)
            {
                string type = domainEventNotification.GetType().FullName;
                var data = JsonConvert.SerializeObject(domainEventNotification);
                OutboxMessage outboxMessage = new OutboxMessage(
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);
                this._context.OutboxMessages.Add(outboxMessage);
            }
        }
    }
}
