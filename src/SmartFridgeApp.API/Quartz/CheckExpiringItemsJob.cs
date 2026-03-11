using System;
using System.Threading.Tasks;
using Dapper;
using Quartz;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.API.Quartz;

/// <summary>
/// Scheduled job that checks for fridge items expiring within 3 days
/// and notifies their owners. Also applies a daily score penalty for
/// items that have already expired but remain active.
/// Runs daily at 8:00 AM (cron: 0 0 8 * * ?).
/// </summary>
public class CheckExpiringItemsJob(
    ISqlConnectionFactory sqlConnectionFactory,
    INotifier notifier) : IJob
{
    private const int ExpiryWarningDays = 3;
    private const int ExpiredPenaltyPoints = -5;

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("[CheckExpiringItemsJob] Checking for expiring fridge items...");
        var connection = sqlConnectionFactory.GetOpenConnection();

        const string expiringItemsSql = """
            SELECT fp."Name" AS "ProductName",
                   au."Email",
                   au."Name" AS "UserName",
                   fi."ExpirationDate",
                   EXTRACT(DAY FROM fi."ExpirationDate" - NOW())::int AS "DaysUntilExpiry"
            FROM app."FridgeItems" fi
            JOIN app."FridgeMembers" fm ON fi."MemberId" = fm."Id"
            JOIN app."AppUsers" au ON fm."Email" = au."Email"
            JOIN app."FoodProducts" fp ON fi."FoodProductId" = fp."FoodProductId"
            WHERE fi."IsConsumed" = false
              AND fi."IsWasted" = false
              AND fi."ExpirationDate" <= NOW() + INTERVAL '1 day' * @Days
            ORDER BY fi."ExpirationDate" ASC
            """;

        var expiringItems = await connection.QueryAsync<ExpiringNotificationDto>(
            expiringItemsSql, new { Days = ExpiryWarningDays });

        foreach (var item in expiringItems)
        {
            var message = item.DaysUntilExpiry <= 0
                ? $"Your \"{item.ProductName}\" has expired!"
                : $"Your \"{item.ProductName}\" expires in {item.DaysUntilExpiry} day(s).";

            notifier.SendMessage(item.Email, message);
        }

        // Apply score penalty for items that expired in the last 24 hours
        const string expiredPenaltySql = """
            UPDATE app."FridgeMembers" fm
            SET "WasteScore" = "WasteScore" + @Penalty
            WHERE fm."Id" IN (
                SELECT DISTINCT fi."MemberId"
                FROM app."FridgeItems" fi
                WHERE fi."IsConsumed" = false
                  AND fi."IsWasted" = false
                  AND fi."ExpirationDate" < NOW()
                  AND fi."ExpirationDate" >= NOW() - INTERVAL '1 day'
            )
            """;

        var affected = await connection.ExecuteAsync(expiredPenaltySql, new { Penalty = ExpiredPenaltyPoints });
        if (affected > 0)
        {
            Console.WriteLine($"[CheckExpiringItemsJob] Applied expiry penalty to {affected} member(s).");
        }

        Console.WriteLine("[CheckExpiringItemsJob] Done.");
    }

    private sealed class ExpiringNotificationDto
    {
        public string Email { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string ProductName { get; init; } = string.Empty;
        public DateTime ExpirationDate { get; init; }
        public int DaysUntilExpiry { get; init; }
    }
}
