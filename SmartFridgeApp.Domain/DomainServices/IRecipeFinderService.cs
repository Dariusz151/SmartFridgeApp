using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.Domain.DomainServices
{
    public interface IRecipeFinderService
    {
        Task<List<Recipe>> FindMatchingRecipes(List<FridgeItem> fridgeItems);
    }
}
