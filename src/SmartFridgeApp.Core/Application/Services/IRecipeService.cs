using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;

namespace SmartFridgeApp.Core.Application.Services;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDto>> GetRecipesAsync(CancellationToken ct = default);
    Task<Recipe> AddRecipeAsync(string name, List<FoodProductDetailsDto> products, string description, int recipeCategory, int requiredTime, int levelOfDifficulty, CancellationToken ct = default);
    Task UpdateRecipeAsync(Guid recipeId, string name, string description, int recipeCategory, int requiredTime, int levelOfDifficulty, CancellationToken ct = default);
    Task DeleteRecipeAsync(Guid recipeId, CancellationToken ct = default);
    Task<IEnumerable<Recipe>> FindRecipesForKitchenAsync(Guid kitchenId, List<short> selectedFoodProductIds, CancellationToken ct = default);
    Task<List<FoodProductDetails>> GetMissingProductsAsync(Guid kitchenId, Guid recipeId, CancellationToken ct = default);
    Task AddMissingProductsToShoppingListAsync(Guid kitchenId, Guid recipeId, string userEmail, CancellationToken ct = default);
    Task<IEnumerable<RecipeCategory>> GetRecipeCategoriesAsync(CancellationToken ct = default);
    Task CreateRecipeCategoryAsync(string name, CancellationToken ct = default);
}
