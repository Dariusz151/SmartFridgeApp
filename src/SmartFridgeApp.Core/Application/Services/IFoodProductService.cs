using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFoodProductService
{
    Task<IEnumerable<FoodProductDto>> GetFoodProductsAsync(CancellationToken ct = default);
    Task AddFoodProductAsync(string name, int categoryId, CancellationToken ct = default);
    Task UpdateFoodProductAsync(int foodProductId, string foodProductName, CancellationToken ct = default);
    Task DeleteFoodProductAsync(int foodProductId, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken ct = default);
    Task CreateCategoryAsync(string name, CancellationToken ct = default);
}
