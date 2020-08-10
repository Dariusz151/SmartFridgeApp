﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Dapper;
using Quartz;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.InternalCommands;

namespace SmartFridgeApp.API.InternalCommands
{
    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessInternalCommandsJob(
            ILifetimeScope lifetimeScope, ISqlConnectionFactory sqlConnectionFactory)
        {
            _lifetimeScope = lifetimeScope;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[Command].[Id] " +
                               "FROM [app].[InternalCommands] AS [Command] " +
                               "WHERE [Command].[ProcessedDate] IS NULL";
            var commandIds = await connection.QueryAsync<Guid>(sql);

            var commandListIds = commandIds.AsList();

            foreach (var commandId in commandListIds)
            {
                using (var scope = _lifetimeScope.BeginLifetimeScope())
                {
                    await scope.Resolve<ICommandsDispatcher>().DispatchCommandAsync(commandId);
                }
            }
        }
    }
}
