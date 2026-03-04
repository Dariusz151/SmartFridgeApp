using System.Threading;
using System.Threading.Tasks;

namespace SmartFridgeApp.Infrastructure.SeedWork;

public interface IDomainEventNotificationHandler<in TNotification>
{
    Task HandleAsync(TNotification notification, CancellationToken ct = default);
}
