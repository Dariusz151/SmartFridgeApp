using System.Data;

namespace SmartFridgeApp.Infrastructure.SeedWork
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
