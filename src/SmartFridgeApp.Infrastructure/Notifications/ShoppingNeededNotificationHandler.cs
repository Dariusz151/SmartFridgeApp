using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class ShoppingNeededNotificationHandler(INotifier notifier)
    : IDomainEventNotificationHandler<ShoppingNeededNotification>
{
    public Task HandleAsync(ShoppingNeededNotification notification, CancellationToken ct = default)
    {
        var message = $"Your fridge is running low! " +
                      $"Only {notification.ActiveItemCount} items left " +
                      $"(average: {notification.AverageItemCount:F1}). Time to go shopping!";

        notifier.SendMessage(notification.FridgeId.ToString(), message);
        return Task.CompletedTask;
    }
}
