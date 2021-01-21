using System;
using MediatR;

namespace SmartFridgeApp.API.Recipes.DeleteRecipe
{
    public class DeleteRecipeCommand : IRequest
    {
        public Guid RecipeId { get; set; }

        public DeleteRecipeCommand(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
