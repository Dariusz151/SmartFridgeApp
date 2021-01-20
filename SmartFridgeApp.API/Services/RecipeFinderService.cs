using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.DomainServices;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.API.Services
{
    public class RecipeFinderService : IRecipeFinderService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeFinderService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<List<Recipe>> FindMatchingRecipes(List<short> foodProducts)
        {
            var recipes = await _recipeRepository.GetAllRecipesAsync();

            var recipesAvailable = new List<Recipe>();
            foreach (var recipe in recipes)
            {
                // TODO: test this change? Added Where condition
                var listOfIds = recipe.FoodProducts.Where(x=>x.IsOptional == false).Select(x => x.FoodProductId).ToList();
                if (!listOfIds.Except(foodProducts).Any())
                {
                    recipesAvailable.Add(recipe);
                }
            }

            return recipesAvailable;
        }
    }
}
