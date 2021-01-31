using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetRecipesQuery : IRequest<IEnumerable<RecipeDto>>
    {
        public GetRecipesQuery()
        {
            
        }
    }
}
