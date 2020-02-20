using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == categoryId);
        }
    }
}
