using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartFridgeAppContext _smartFridgeAppContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(SmartFridgeAppContext smartFridgeAppContext, IDomainEventsDispatcher domainEventsDispatcher)
        {
            _smartFridgeAppContext = smartFridgeAppContext;
            _domainEventsDispatcher = domainEventsDispatcher;
        }
        
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this._domainEventsDispatcher.DispatchEventsAsync();
            return await this._smartFridgeAppContext.SaveChangesAsync(cancellationToken);
        }
    }
}
