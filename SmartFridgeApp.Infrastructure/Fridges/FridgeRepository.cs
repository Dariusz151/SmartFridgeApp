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

        public async Task<Fridge> GetByIdAsync(FridgeId id)
        {
            return await _context.Fridges.SingleAsync(x => x.Id == id);
        }
    }
}
