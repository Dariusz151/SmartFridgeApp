using Npgsql;
using SmartFridgeApp.Infrastructure.SeedWork;
using SmartFridgeApp.Shared.SeedWork;
using System;
using System.Data;
using Microsoft.Extensions.Options;
using SmartFridgeApp.Infrastructure.Database;

namespace SmartFridgeApp.Infrastructure
{
    public class SqlConnectionFactory(IOptions<DatabaseOptions> options) : ISqlConnectionFactory, IDisposable
    {
        private IDbConnection _connection;

        public IDbConnection GetOpenConnection()
        {
            if (this._connection == null || this._connection.State != ConnectionState.Open)
            {
                this._connection = new NpgsqlConnection(options.Value.ConnectionString);
                this._connection.Open();
            }

            return this._connection;
        }

        public void Dispose()
        {
            if (this._connection != null && this._connection.State == ConnectionState.Open)
            {
                this._connection.Dispose();
            }
        }
    }
}
