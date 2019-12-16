using System.Threading.Tasks;

namespace SmartFridgeApp.Infrastructure
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}