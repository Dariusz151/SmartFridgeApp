using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Quartz;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Inventory;
using SmartFridgeApp.Core.Domain.Inventory.Events;
using SmartFridgeApp.Infrastructure.Inventory;

namespace SmartFridgeApp.API.Quartz;

public class CheckExpiringItemsJob(
    IQuerySession querySession,
    IKitchenInventoryRepository inventoryRepository,
    INotifier notifier) : IJob
{
    private const int ExpiryWarningDays = 3;

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("[CheckExpiringItemsJob] Checking for expiring Kitchen items...");

        var threshold = DateTimeOffset.UtcNow.AddDays(ExpiryWarningDays);
        var expiringItems = await querySession.Query<ActiveStockItemDocument>()
            .Where(d => d.ExpirationDate <= threshold)
            .ToListAsync();

        foreach (var item in expiringItems)
        {
            var daysUntilExpiry = (int)(item.ExpirationDate - DateTimeOffset.UtcNow).TotalDays;
            var message = daysUntilExpiry <= 0
                ? $"Product {item.FoodProductId} has expired!"
                : $"Product {item.FoodProductId} expires in {daysUntilExpiry} day(s).";

            notifier.SendMessage(item.MemberId.ToString(), message);
        }

        var expiredItems = expiringItems.Where(d => d.ExpirationDate < DateTimeOffset.UtcNow).ToList();
        foreach (var expired in expiredItems)
        {
            var (inventory, version) = await inventoryRepository.LoadAsync(expired.KitchenId);
            var evt = new ItemExpired(expired.Id, DateTimeOffset.UtcNow);
            inventory.Apply(evt);
            await inventoryRepository.AppendEventsAsync(expired.KitchenId, version, [evt]);
        }

        if (expiredItems.Count > 0)
            Console.WriteLine($"[CheckExpiringItemsJob] Recorded {expiredItems.Count} expiry event(s).");

        Console.WriteLine("[CheckExpiringItemsJob] Done.");
    }
}
