using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;
using System.Collections.Generic;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Infrastructure.Kitchens
{
    public class KitchenRepository : IKitchenRepository
    {
        private readonly SmartFridgeAppContext _context;

        public KitchenRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KitchenDto>> GetAllFridgesAsync()
        {
            return await _context.Kitchens
                .AsNoTracking()
                .Select(f => new KitchenDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Address = f.Address,
                    Desc = f.Desc,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();
        }

        public async Task AddAsync(Kitchen Kitchen)
        {
            await _context.Kitchens.AddAsync(Kitchen);
        }

        public async Task DeleteAsync(Guid kitchenId)
        {
            var Kitchen = await GetByIdAsync(kitchenId);
            _context.Kitchens.Remove(Kitchen);
        }


        public async Task<Kitchen> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Kitchens.Where(x => x.Id == id).SingleAsync();
            }
            catch
            {
                throw new InvalidKitchenException("This Kitchen id does not exist.", "InvalidKitchenId");
            }
        }
    }
}
