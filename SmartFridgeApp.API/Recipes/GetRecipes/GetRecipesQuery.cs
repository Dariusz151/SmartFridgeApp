using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.API.Recipes.GetRecipes
{
    public class GetRecipesQuery : IRequest<IEnumerable<RecipeDto>>
    {
        public GetRecipesQuery()
        {
            
        }
    }
}
