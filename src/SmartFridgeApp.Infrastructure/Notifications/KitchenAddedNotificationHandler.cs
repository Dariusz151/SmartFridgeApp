using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class KitchenAddedNotificationHandler(INotifier notifier) : IDomainEventNotificationHandler<KitchenAddedNotification>
{
    public Task HandleAsync(KitchenAddedNotification notification, CancellationToken ct = default)
    {
        var kitchenName = notification.kitchenId.ToString();
        notifier.SendMessage(kitchenName, "New Kitchen added to application!");
        return Task.CompletedTask;
    }
}
