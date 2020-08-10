using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Infrastructure.InternalCommands;

namespace SmartFridgeApp.API.Fridges.IntegrationHandlers
{
    public class FridgeAddedNotificationHandler : INotificationHandler<FridgeAddedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public FridgeAddedNotificationHandler(
            ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(FridgeAddedNotification notification, CancellationToken cancellationToken)
        {
            // Send welcome e-mail message...

            await this._commandsScheduler.EnqueueAsync(new MarkFridgeAsWelcomedCommand(notification.FridgeId));
        }
    }
}
