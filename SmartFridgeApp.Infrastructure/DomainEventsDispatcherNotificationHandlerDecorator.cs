﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;

//namespace SmartFridgeApp.Infrastructure
//{
//    public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
//    {
//        private readonly INotificationHandler<T> _decorated;
//        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

//        public DomainEventsDispatcherNotificationHandlerDecorator(
//            IDomainEventsDispatcher domainEventsDispatcher,
//            INotificationHandler<T> decorated)
//        {
//            _domainEventsDispatcher = domainEventsDispatcher;
//            _decorated = decorated;
//        }

//        public async Task Handle(T notification, CancellationToken cancellationToken)
//        {
//            await this._decorated.Handle(notification, cancellationToken);

//            await this._domainEventsDispatcher.DispatchEventsAsync();
//        }
//    }
//}
