using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.Domain.DomainServices
{
    public interface IRecipeFinderService
    {
        Task<List<Recipe>> FindMatchingRecipes(List<short> foodProducts);
    }
}
