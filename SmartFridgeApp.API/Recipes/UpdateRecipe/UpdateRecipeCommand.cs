using MediatR;

namespace SmartFridgeApp.API.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public UpdateRecipeCommand(int recipeId, string name, string description, string category)
        {
            Name = name;
            Description = description;
            Category = category;
            RecipeId = recipeId;
        }
    }
}
