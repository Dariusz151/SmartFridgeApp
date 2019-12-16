using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using SmartFridgeApp.Domain.SeedWork;

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
            
            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await _mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
