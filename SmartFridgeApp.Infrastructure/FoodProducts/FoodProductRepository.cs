using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.FoodProducts;

namespace SmartFridgeApp.Infrastructure.FoodProducts
{
    public class FoodProductRepository : IFoodProductRepository
    {
        private readonly SmartFridgeAppContext _context;

        public FoodProductRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FoodProduct foodProduct)
        {
            await _context.FoodProducts.AddAsync(foodProduct);
        }

        public async Task DeleteAsync(int foodProductId)
        {
            var foodProduct = await _context.FoodProducts.SingleOrDefaultAsync(f => f.Id == foodProductId);

            _context.FoodProducts.Remove(foodProduct);
        }
    }
}
