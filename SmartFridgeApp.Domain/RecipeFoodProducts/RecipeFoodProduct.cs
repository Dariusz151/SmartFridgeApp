using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.Domain.RecipeFoodProducts
{
    public class RecipeFoodProduct
    {
        public int RecipeId { get; set; }
        public int FoodProductId { get; set; }
        public Recipe Recipe { get; set; }
        public FoodProduct FoodProduct { get; set; }
    }
}
