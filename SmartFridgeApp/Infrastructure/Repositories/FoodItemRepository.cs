using SmartFridgeApp.Domain.Models;
using SmartFridgeApp.Domain.Repositories;
using SmartFridgeApp.Persistence;
using System;
using System.Threading.Tasks;

namespace SmartFridgeApp.Infrastructure.Repositories
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly SmartFridgeContext _context;
        private readonly IUserRepository _userRepository;

        public FoodItemRepository(SmartFridgeContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        
        public async Task AddFoodItemAsync(FoodItem item, Guid userId)
        {
            var foodItem = item;
            
            foodItem.AssignToUser(userId);
            
            await _context.FoodItems.AddAsync(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task AddFoodItemToArchiveAsync(FoodItem item, Guid userId)
        {
            var consumedItem = new ConsumedFoodItem(item.Name, userId, item.Amount, item.Unit);

            await _context.ConsumedFoodItems.AddAsync(consumedItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFoodItemAsync(Guid foodItemId)
        {
            var item = await _context.FoodItems.FindAsync(foodItemId);

            _context.FoodItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<FoodItem> GetFoodItemByIdAsync(Guid foodItemId)
        {
            var item = await _context.FoodItems.FindAsync(foodItemId);
            return item;
        }
    }
}
