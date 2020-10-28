using System;

namespace SmartFridgeApp.API.Recipes
{
    public class RecipeDto
    {
        public Guid RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public int RequiredTime { get; set; }
        public int LevelOfDifficultyId { get; set; }
        public string LevelOfDifficulty { get; set; }
        public string RecipeCategory { get; set; }
        public string FoodProducts { get; set; }
    }
}
