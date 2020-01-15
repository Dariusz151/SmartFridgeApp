using System.Collections.Generic;
using SmartFridgeApp.Domain.RecipeFoodProducts;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeRequest
    {
        public string Name { get; set; }
        public List<RecipeFoodProduct> Products { get; set; }
    }
}
