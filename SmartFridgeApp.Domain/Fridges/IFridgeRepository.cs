using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Fridges
{
    public interface IFridgeRepository
    {
        Task<Fridge> GetByIdAsync(Guid id);
        Task AddAsync(Fridge fridge);
        Task DeleteAsync(Guid fridgeId);
        //Task<List<Fridge>> GetAllAsync();
    }
}
