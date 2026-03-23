using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Core.Exceptions;

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
            try
            {
                return await _context.FoodProducts.SingleAsync(f => f.FoodProductId == foodProductId);
            }
            catch
            {
                throw new FoodProductNotFoundException("This FoodProduct does not exist.", "InvalidFoodProductId");
            }
        }

        public async Task<FoodProduct> GetByNameAsync(string foodProductName)
        {
            try
            {
                return await _context.FoodProducts.SingleAsync(f => f.Name == foodProductName);
            }
            catch
            {
                throw new FoodProductNotFoundException("This FoodProduct does not exist.", "InvalidFoodProductName");
            }
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
            var foodProduct = await GetByIdAsync(foodProductId);
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
            try
            {
                return await _context.Categories.SingleAsync(c => c.CategoryId == categoryId);
            }
            catch
            {
                throw new InvalidFoodProductCategoryException("This category does not exist.", "InvalidFoodProductCategoryId");
            }
        }

        public async Task<ProductVariant?> GetVariantByIdAsync(int variantId)
        {
            return await _context.ProductVariants.FirstOrDefaultAsync(v => v.VariantId == variantId);
        }

        public async Task<ProductVariant?> GetVariantByBarcodeAsync(string barcode)
        {
            return await _context.ProductVariants.FirstOrDefaultAsync(v => v.Barcode == barcode);
        }

        public async Task<IReadOnlyList<ProductVariant>> GetVariantsByFoodProductIdAsync(short foodProductId)
        {
            return await _context.ProductVariants
                .Where(v => v.FoodProductId == foodProductId)
                .ToListAsync();
        }

        public async Task AddVariantAsync(ProductVariant variant)
        {
            await _context.ProductVariants.AddAsync(variant);
        }
    }
}
