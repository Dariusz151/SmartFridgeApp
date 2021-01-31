using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetRecipeCategoriesQuery : IRequest<IEnumerable<RecipeCategory>>
    {
        public GetRecipeCategoriesQuery()
        {
            
        }
    }
}
