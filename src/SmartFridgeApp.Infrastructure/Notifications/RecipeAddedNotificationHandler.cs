using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.Infrastructure.Notifications;

public class RecipeAddedNotificationHandler(INotifier notifier) : IDomainEventNotificationHandler<RecipeAddedNotification>
{
    public Task HandleAsync(RecipeAddedNotification notification, CancellationToken ct = default)
    {
        var recipeId = notification.RecipeId.ToString();
        notifier.SendMessage(recipeId, "New recipe added!");
        return Task.CompletedTask;
    }
}
