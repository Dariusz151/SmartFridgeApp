using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts.Repositories
{
    public interface IKitchenRepository
    {
        Task<Kitchen> GetByIdAsync(Guid id);
        Task AddAsync(Kitchen Kitchen);
        Task DeleteAsync(Guid kitchenId);

        Task<IEnumerable<KitchenDto>> GetAllFridgesAsync();
    }
}
