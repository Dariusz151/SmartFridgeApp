using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class FridgeAddedNotificationHandler(INotifier notifier) : IDomainEventNotificationHandler<FridgeAddedNotification>
{
    public Task HandleAsync(FridgeAddedNotification notification, CancellationToken ct = default)
    {
        var fridgeName = notification.FridgeId.ToString();
        notifier.SendMessage(fridgeName, "New fridge added to application!");
        return Task.CompletedTask;
    }
}
