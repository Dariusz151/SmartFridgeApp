using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.Fridges;
using System.Threading.Tasks;

namespace SmartFridgeApp.Infrastructure.Fridges
{
    public class FridgeRepository : IFridgeRepository
    {
        private readonly SmartFridgeAppContext _context;

        public FridgeRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Fridge fridge)
        {
            await _context.Fridges.AddAsync(fridge);
        }

        public async Task<Fridge> GetByIdAsync(Guid id)
        {
            return await _context.Fridges.SingleAsync(x => x.Id == id);
        }

        //public async Task<List<Fridge>> GetAllAsync()
        //{
        //    return await _context.Fridges.ToListAsync();
        //}

        public async Task DeleteAsync(Guid fridgeId)
        {
            var fridge = await _context.Fridges.SingleOrDefaultAsync(f => f.Id == fridgeId);

            _context.Fridges.Remove(fridge);
        }
    }
}
