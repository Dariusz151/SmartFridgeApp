using SmartFridgeApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts.Repositories
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetRecipeByIdAsync(Guid recipeId);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task AddRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(Guid recipeId);
        Task CreateRecipeCategoryAsync(RecipeCategory recipeCategory);
        Task<IEnumerable<RecipeCategory>> GetAllRecipeCategoriesAsync();
        Task<RecipeCategory> GetRecipeCategoryByIdAsync(int recipeCategoryId);
    }
}
