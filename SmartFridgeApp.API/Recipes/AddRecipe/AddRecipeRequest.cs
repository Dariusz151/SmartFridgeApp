using System.Collections.Generic;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RecipeCategory { get; set; }
        public List<FoodProductDetails> Products { get; set; }
        //public List<int> ProductIds { get; set; }
    }
}
