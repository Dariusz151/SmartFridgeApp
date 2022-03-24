using System.Threading.Tasks;

namespace SmartFridgeApp.Infrastructure.SeedWork
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}