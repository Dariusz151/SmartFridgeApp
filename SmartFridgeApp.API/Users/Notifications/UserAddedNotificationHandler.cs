using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.API.Notifications;

namespace SmartFridgeApp.API.Users.Notifications
{
    public class UserAddedNotificationHandler : INotificationHandler<UserAddedNotification>
    {
        private readonly INotifier _notifier;

        public UserAddedNotificationHandler(INotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task Handle(UserAddedNotification notification, CancellationToken cancellationToken)
        {
            var userId = notification.UserId.ToString();
            
            _notifier.SendMessage(userId, $"Welcome in SmartFridgeApp {userId}!");

            await Task.CompletedTask;
        }
    }
}
