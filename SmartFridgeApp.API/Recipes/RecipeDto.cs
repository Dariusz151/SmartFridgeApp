using System.Collections.Generic;
using SmartFridgeApp.API.FoodProducts;

namespace SmartFridgeApp.API.Recipes
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public int DifficultyLevel { get; set; }
        public int MinutesRequired { get; set; }
        public string Category { get; set; }
        public ICollection<FoodProductDto> FoodProducts { get; set; }
    }
}
