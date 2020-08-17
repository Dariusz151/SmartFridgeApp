using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.InternalCommands;

namespace SmartFridgeApp.API.InternalCommands
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync(IRequest command)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [app].[InternalCommands] ([Id], [ProcessedDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            try
            {
                await connection.ExecuteAsync(sqlInsert, new
                {
                    Id = Guid.NewGuid(),
                    EnqueueDate = DateTime.UtcNow,
                    Type = command.GetType().FullName,
                    Data = JsonConvert.SerializeObject(command)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            
        }
    }
}
