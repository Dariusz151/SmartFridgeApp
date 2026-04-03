using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFoodProductService
{
    Task<IEnumerable<FoodProductDto>> GetFoodProductsAsync(CancellationToken ct = default);
    Task AddFoodProductAsync(string name, int categoryId, StorageLocation? defaultStorageLocation = null, Unit? defaultUnit = null, CancellationToken ct = default);
    Task UpdateFoodProductAsync(int foodProductId, string foodProductName, StorageLocation? defaultStorageLocation = null, Unit? defaultUnit = null, CancellationToken ct = default);
    Task DeleteFoodProductAsync(int foodProductId, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken ct = default);
    Task CreateCategoryAsync(string name, CancellationToken ct = default);

    // Variants
    Task<IReadOnlyList<ProductVariantDto>> GetVariantsAsync(short foodProductId, CancellationToken ct = default);
    Task AddVariantAsync(short foodProductId, string name, string? barcode, CancellationToken ct = default);
}
