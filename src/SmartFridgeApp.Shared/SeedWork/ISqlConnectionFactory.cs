using System.Data;

namespace SmartFridgeApp.Shared.SeedWork
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
