using System.Collections.Generic;
using SmartFridgeApp.Domain.Models.FridgeItems;

namespace SmartFridgeApp.API.Recipes.FindRecipes
{
    public class FindRecipesRequest
    {
        public List<int> FoodProducts { get; set; }
    }
}
