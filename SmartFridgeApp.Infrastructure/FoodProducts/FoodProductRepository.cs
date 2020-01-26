using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.Models.FoodProducts;

namespace SmartFridgeApp.Infrastructure.FoodProducts
{
    public class FoodProductRepository : IFoodProductRepository
    {
        private readonly SmartFridgeAppContext _context;

        public FoodProductRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task<FoodProduct> GetByIdAsync(int foodProductId)
        {
            return await _context.FoodProducts.SingleOrDefaultAsync(f=> f.FoodProductId == foodProductId);
        }

        public async Task AddAsync(FoodProduct foodProduct)
        {
            await _context.FoodProducts.AddAsync(foodProduct);
        }

        public async Task<ICollection<FoodProduct>> GetAllAsync()
        {
            return await _context.FoodProducts.ToListAsync();
        }

        public async Task DeleteAsync(int foodProductId)
        {
            var foodProduct = await _context.FoodProducts.SingleAsync(x => x.FoodProductId == foodProductId);
            _context.FoodProducts.Remove(foodProduct);
        }

        //public async Task DeleteAsync(int foodProductId)
        //{
        //    var foodProduct = await _context.FoodProducts.SingleOrDefaultAsync(f => f.Id == foodProductId);

        //    _context.FoodProducts.Remove(foodProduct);
        //}
    }
}
