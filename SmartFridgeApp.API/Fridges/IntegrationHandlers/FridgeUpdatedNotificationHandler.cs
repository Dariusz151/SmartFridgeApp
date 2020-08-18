using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Infrastructure.InternalCommands;

namespace SmartFridgeApp.API.Fridges.IntegrationHandlers
{
    public class FridgeUpdatedNotificationHandler : INotificationHandler<FridgeUpdatedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public FridgeUpdatedNotificationHandler(
            ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(FridgeUpdatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("FridgeUpdatedNotification.");
            //await this._commandsScheduler.EnqueueAsync(new MarkFridgeAsWelcomedCommand(notification.FridgeId));
        }
    }
}
