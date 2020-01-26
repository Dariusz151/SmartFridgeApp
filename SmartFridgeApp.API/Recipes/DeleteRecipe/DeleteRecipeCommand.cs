using MediatR;

namespace SmartFridgeApp.API.Recipes.DeleteRecipe
{
    public class DeleteRecipeCommand : IRequest
    {
        public int RecipeId { get; set; }

        public DeleteRecipeCommand(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
