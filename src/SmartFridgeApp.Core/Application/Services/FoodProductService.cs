using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class FoodProductService(
    IFoodProductRepository foodProductRepository,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IFoodProductService
{
    public async Task<IEnumerable<FoodProductDto>> GetFoodProductsAsync(CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fp."FoodProductId", fp."Name" AS "FoodProductName",
                   c."Name" AS "FoodProductCategory",
                   (SELECT COUNT(*) FROM app."ProductVariants" pv WHERE pv."FoodProductId" = fp."FoodProductId") AS "VariantCount",
                   fp."DefaultStorageLocation",
                   fp."DefaultUnit"
            FROM app."FoodProducts" fp
            LEFT JOIN app."Categories" c ON fp."CategoryId" = c."CategoryId"
            """;

        var foodProducts = await connection.QueryAsync<FoodProductDto>(sql);
        return foodProducts.AsEnumerable();
    }

    public async Task AddFoodProductAsync(string name, int categoryId, StorageLocation? defaultStorageLocation = null, Unit? defaultUnit = null, CancellationToken ct = default)
    {
        var category = await foodProductRepository.GetCategoryByIdAsync(categoryId);
        var foodProduct = new FoodProduct(name, category, defaultStorageLocation, defaultUnit);

        await foodProductRepository.AddAsync(foodProduct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task UpdateFoodProductAsync(int foodProductId, string foodProductName, StorageLocation? defaultStorageLocation = null, Unit? defaultUnit = null, CancellationToken ct = default)
    {
        var foodProduct = await foodProductRepository.GetByIdAsync(foodProductId);
        foodProduct.UpdateProductName(foodProductName);
        foodProduct.UpdateDefaults(defaultStorageLocation, defaultUnit);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task DeleteFoodProductAsync(int foodProductId, CancellationToken ct = default)
    {
        await foodProductRepository.DeleteAsync(foodProductId);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "CategoryId", "Name"
            FROM app."Categories"
            """;

        var categories = await connection.QueryAsync<Category>(sql);
        return categories.AsEnumerable();
    }

    public async Task CreateCategoryAsync(string name, CancellationToken ct = default)
    {
        var category = new Category(name);
        await foodProductRepository.CreateCategoryAsync(category);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<IReadOnlyList<ProductVariantDto>> GetVariantsAsync(short foodProductId, CancellationToken ct = default)
    {
        var variants = await foodProductRepository.GetVariantsByFoodProductIdAsync(foodProductId);
        return variants.Select(v => new ProductVariantDto
        {
            VariantId = v.VariantId,
            FoodProductId = v.FoodProductId,
            Name = v.Name,
            Barcode = v.Barcode,
        }).ToList();
    }

    public async Task AddVariantAsync(short foodProductId, string name, string? barcode, CancellationToken ct = default)
    {
        var foodProduct = await foodProductRepository.GetByIdAsync(foodProductId);
        var variant = foodProduct.AddVariant(name, barcode);
        await foodProductRepository.AddVariantAsync(variant);
        await unitOfWork.CommitAsync(ct);
    }
}
