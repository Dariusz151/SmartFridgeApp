using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Application.Services;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDto>> GetRecipesAsync(CancellationToken ct = default);
    Task<Recipe> AddRecipeAsync(string name, List<FoodProductDetailsDto> products, string description, int recipeCategory, int requiredTime, int levelOfDifficulty, CancellationToken ct = default);
    Task UpdateRecipeAsync(Guid recipeId, string name, string description, int recipeCategory, int requiredTime, int levelOfDifficulty, CancellationToken ct = default);
    Task DeleteRecipeAsync(Guid recipeId, CancellationToken ct = default);
    Task<IEnumerable<Recipe>> FindRecipesAsync(List<short> foodProducts, CancellationToken ct = default);
    Task<IEnumerable<RecipeCategory>> GetRecipeCategoriesAsync(CancellationToken ct = default);
    Task CreateRecipeCategoryAsync(string name, CancellationToken ct = default);
}
