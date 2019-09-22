using SmartFridgeApp.Domain.Requests.Commands;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Services
{
    public interface IFoodItemService
    {
        Task AddFoodItemAsync(AddFoodItemCommand command);
        Task ConsumeFoodItemAsync(ConsumeFoodItemCommand command);
        Task DeleteFoodItemAsync(DeleteFoodItemCommand command);
    }
}
