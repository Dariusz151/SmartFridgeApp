using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features
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
