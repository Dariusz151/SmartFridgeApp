using System;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Fridges
{
    public interface IFridgeRepository
    {
        Task<Fridge> GetByIdAsync(Guid id);
        Task AddAsync(Fridge fridge);
    }
}
