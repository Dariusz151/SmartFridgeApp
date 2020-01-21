using System.Collections.Generic;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DifficultyLevel { get; set; }
        public int MinutesRequired { get; set; }
        public string Category { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
