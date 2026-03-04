using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class UserAddedNotificationHandler(INotifier notifier) : IDomainEventNotificationHandler<UserAddedNotification>
{
    public Task HandleAsync(UserAddedNotification notification, CancellationToken ct = default)
    {
        var userId = notification.UserId.ToString();
        notifier.SendMessage(userId, $"Welcome in SmartFridgeApp {userId}!");
        return Task.CompletedTask;
    }
}
