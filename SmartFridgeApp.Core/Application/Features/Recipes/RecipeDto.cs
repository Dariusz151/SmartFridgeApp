using System;

namespace SmartFridgeApp.Core.Application.Features
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
