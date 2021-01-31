using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Core.Contracts.DomainServices;

namespace SmartFridgeApp.API.Notifications.Fridge
{
    public class FridgeAddedNotificationHandler : INotificationHandler<FridgeAddedNotification>
    {
        private readonly INotifier _notifier;

        public FridgeAddedNotificationHandler(INotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task Handle(FridgeAddedNotification notification, CancellationToken cancellationToken)
        {
            var fridgeName = notification.FridgeId.ToString();
            _notifier.SendMessage(fridgeName, "New fridge added to application!");
        }
    }
}
