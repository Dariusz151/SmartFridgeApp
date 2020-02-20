using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Models.Fridges;

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

        public async Task<Fridge> GetByIdAsync(int id)
        {
            return await _context.Fridges.SingleAsync(x => x.Id == id);
        }
        
        public async Task DeleteAsync(int fridgeId)
        {
            var fridge = await _context.Fridges.SingleOrDefaultAsync(f => f.Id == fridgeId);

            _context.Fridges.Remove(fridge);
        }
    }
}
