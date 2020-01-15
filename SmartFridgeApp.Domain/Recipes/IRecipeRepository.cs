using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Recipes
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAllRecipesAsync();
        Task AddRecipeAsync(Recipe recipe);
    }
}
