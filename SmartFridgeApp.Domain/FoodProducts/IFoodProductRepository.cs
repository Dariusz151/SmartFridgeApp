using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.FoodProducts
{
    public interface IFoodProductRepository
    {
        Task<FoodProduct> GetAllAsync();
        Task AddAsync(FoodProduct foodProduct);
        Task DeleteAsync(int foodProductId);
        // edit ?
    }
}
