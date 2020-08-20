using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.API.Notifications;
using SmartFridgeApp.Infrastructure.InternalCommands;

namespace SmartFridgeApp.API.Fridges.IntegrationHandlers
{
    public class FridgeAddedNotificationHandler : INotificationHandler<FridgeAddedNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly INotifier _notifier;

        public FridgeAddedNotificationHandler(
            ICommandsScheduler commandsScheduler, INotifier notifier)
        {
            _commandsScheduler = commandsScheduler;
            _notifier = notifier;
        }

        public async Task Handle(FridgeAddedNotification notification, CancellationToken cancellationToken)
        {
            var fridgeName = notification.FridgeId.ToString();
            _notifier.SendMessage(fridgeName, "Welcome in SmartFridgeApp!");
            
            await this._commandsScheduler.EnqueueAsync(new MarkFridgeAsWelcomedCommand(notification.FridgeId));
        }
    }
}
