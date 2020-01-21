using System.Collections.Generic;
using SmartFridgeApp.API.FoodProducts;

namespace SmartFridgeApp.API.Recipes
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public ICollection<FoodProductDto> RecipeFoodProducts { get; set; }
    }
}
