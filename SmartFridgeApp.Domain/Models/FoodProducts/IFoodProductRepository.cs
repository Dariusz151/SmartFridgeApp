using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public interface IFoodProductRepository
    {
        Task<FoodProduct> GetByIdAsync(int foodProductId);
        Task AddAsync(FoodProduct foodProduct);
        Task<ICollection<FoodProduct>> GetAllAsync();
        Task DeleteAsync(int foodProductId);
    }
}
