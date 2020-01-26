using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Models.Recipes
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task AddRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int recipeId);
    }
}
