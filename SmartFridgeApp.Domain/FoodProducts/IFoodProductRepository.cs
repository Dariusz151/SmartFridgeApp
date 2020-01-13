using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.FoodProducts
{
    public interface IFoodProductRepository
    {
        Task<FoodProduct> GetByIdAsync(int foodProductId);
        Task AddAsync(FoodProduct foodProduct);
    }
}
