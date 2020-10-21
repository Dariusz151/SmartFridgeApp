using System;
using MediatR;

namespace SmartFridgeApp.API.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RecipeCategory { get; set; }

        public UpdateRecipeCommand(Guid recipeId, string name, string description, int category)
        {
            Name = name;
            Description = description;
            RecipeCategory = category;
            RecipeId = recipeId;
        }
    }
}
