using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts.Repositories
{
    public interface IFridgeRepository
    {
        Task<Fridge> GetByIdAsync(Guid id);
        Task AddAsync(Fridge fridge);
        Task DeleteAsync(Guid fridgeId);

        Task<IEnumerable<FridgeDto>> GetAllFridgesAsync();
    }
}
