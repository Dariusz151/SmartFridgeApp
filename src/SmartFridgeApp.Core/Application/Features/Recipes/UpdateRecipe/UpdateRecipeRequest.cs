using System;

namespace SmartFridgeApp.Core.Application.Features
{
    public class UpdateRecipeRequest
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RequiredTime { get; set; }
        public int LevelOfDifficulty { get; set; }
        public int Category { get; set; }
    }
}
