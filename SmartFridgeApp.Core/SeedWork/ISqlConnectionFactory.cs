using System.Data;

namespace SmartFridgeApp.Core.SeedWork
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
