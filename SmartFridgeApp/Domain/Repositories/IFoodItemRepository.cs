using SmartFridgeApp.Domain.Models;
using System;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Repositories
{
    public interface IFoodItemRepository
    {
        Task<FoodItem> GetFoodItemByIdAsync(Guid foodItemId);

        Task AddFoodItemAsync(FoodItem item, Guid userId);
        Task DeleteFoodItemAsync(Guid foodItemId);
        Task AddFoodItemToArchiveAsync(FoodItem item, Guid userId);
        //Task UpdateFoodItemAsync(FoodItem item, Guid userId);
    }
}
