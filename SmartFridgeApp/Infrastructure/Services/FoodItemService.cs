using System;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Models;
using SmartFridgeApp.Domain.Repositories;
using SmartFridgeApp.Domain.Requests.Commands;
using SmartFridgeApp.Domain.Services;

namespace SmartFridgeApp.Infrastructe.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _foodItemRepository;

        public FoodItemService(IFoodItemRepository foodItemRepository)
        {
            _foodItemRepository = foodItemRepository;
        }

        public async Task AddFoodItemAsync(AddFoodItemCommand command)
        {
            var item = new FoodItem(command.Name,
                                    command.Amount,
                                    command.Unit,
                                    command.ExpirationDate,
                                    command.Category
                );
            
            await _foodItemRepository.AddFoodItemAsync(item, command.UserId);
        }

        public async Task ConsumeFoodItemAsync(ConsumeFoodItemCommand command)
        {
            var foodItem = await _foodItemRepository.GetFoodItemByIdAsync(command.FoodItemId);

            var userId = command.UserId;

            await _foodItemRepository.AddFoodItemToArchiveAsync(foodItem, userId);

            await _foodItemRepository.DeleteFoodItemAsync(foodItem.Id);
        }

        public async Task DeleteFoodItemAsync(DeleteFoodItemCommand command)
        {
            await _foodItemRepository.DeleteFoodItemAsync(command.Id);
        }
    }
}
