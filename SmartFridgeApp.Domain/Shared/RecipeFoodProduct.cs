using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.Domain.Shared
{
    public class RecipeFoodProduct
    {
        // for EF Core Many-to-Many purpose

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int FoodProductId { get; set; }
        public FoodProduct FoodProduct { get; set; }
    }
}
