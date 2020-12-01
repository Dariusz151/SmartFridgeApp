using System.Collections.Generic;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecipeCategory { get; set; }
        public int RequiredTime { get; set; }
        public int LevelOfDifficulty { get; set; }
        public List<FoodProductDetailsDto> Products { get; set; }
    }
}
