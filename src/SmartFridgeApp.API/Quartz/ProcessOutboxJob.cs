using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using SmartFridgeApp.Infrastructure.Notifications;
using SmartFridgeApp.Infrastructure.SeedWork;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.API.Quartz
{
    public class ProcessOutboxJob(
        IServiceProvider serviceProvider,
        ISqlConnectionFactory sqlConnectionFactory) : IJob
    {
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.All
        };

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Execute ProcessOutboxJob");
            var connection = sqlConnectionFactory.GetOpenConnection();

            const string sql = """
                SELECT om."Id", om."Type", om."Data"
                FROM internal."OutboxMessages" om
                WHERE om."ProcessedDate" IS NULL
                """;

            var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

            const string sqlUpdateProcessedDate = """
                UPDATE internal."OutboxMessages"
                SET "ProcessedDate" = @Date
                WHERE "Id" = @Id
                """;

            foreach (var message in messages)
            {
                Type type = Assembly.GetAssembly(typeof(FridgeAddedNotification))!.GetType(message.Type)!;
                var notification = JsonConvert.DeserializeObject(message.Data, type, JsonSettings);

                Console.WriteLine(notification);

                if (notification != null)
                {
                    await DispatchNotificationAsync(notification);
                }

                await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                {
                    Date = DateTime.UtcNow,
                    message.Id
                });
            }
        }

        private async Task DispatchNotificationAsync(object notification)
        {
            var notificationType = notification.GetType();
            var handlerType = typeof(IDomainEventNotificationHandler<>).MakeGenericType(notificationType);
            var handler = serviceProvider.GetService(handlerType);

            if (handler == null) return;

            var method = handlerType.GetMethod(nameof(IDomainEventNotificationHandler<object>.HandleAsync))!;
            await (Task)method.Invoke(handler, [notification, CancellationToken.None])!;
        }
    }
}
