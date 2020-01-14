using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Recipes
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetAllRecipesAsync();
    }
}
