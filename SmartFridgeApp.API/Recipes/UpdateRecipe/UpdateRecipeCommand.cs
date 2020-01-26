using MediatR;

namespace SmartFridgeApp.API.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DifficultyLevel { get; set; }
        public int MinutesRequired { get; set; }
        public string Category { get; set; }

        public UpdateRecipeCommand(int recipeId, string name, string description, int difficultyLevel, int minutesRequired, string category)
        {
            Name = name;
            Description = description;
            DifficultyLevel = difficultyLevel;
            MinutesRequired = minutesRequired;
            Category = category;
            RecipeId = recipeId;
        }
    }
}
