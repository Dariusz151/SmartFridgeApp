using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.DomainServices;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.Recipes
{
    public class RecipeFinderService : IRecipeFinderService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeFinderService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<List<Recipe>> FindMatchingRecipes(List<int> foodProducts)
        {
            var recipes = await _recipeRepository.GetAllRecipesAsync();

            var recipesAvailable = new List<Recipe>();
            //var productAgreedCounter = 0;

            foreach (var recipe in recipes)
            {
                recipesAvailable.Add(recipe);

                //// TODO: Convert into LINQ Expression
                //foreach (var foodProduct in foodProducts)
                //{
                //    var result = recipe.RecipeFoodProducts.Where(x => x.FoodProductId == foodProduct).ToList();
                //    if (result.Count > 0)
                //        productAgreedCounter++;
                //}

                //if (productAgreedCounter >= recipe.RecipeFoodProducts.Count)
                //{
                //    recipesAvailable.Add(recipe);
                //    productAgreedCounter = 0;
                //}
            }

            return recipesAvailable;
        }
    }
}
