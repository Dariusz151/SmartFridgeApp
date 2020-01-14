using System.Threading.Tasks;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    public class RecipeRepository : IRecipeRepository
    {
        public Task<Recipe> GetAllRecipesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
