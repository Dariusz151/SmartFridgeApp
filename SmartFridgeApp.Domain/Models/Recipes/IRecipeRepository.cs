using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Models.Recipes
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetRecipeByIdAsync(Guid recipeId);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task AddRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(Guid recipeId);
    }
}
