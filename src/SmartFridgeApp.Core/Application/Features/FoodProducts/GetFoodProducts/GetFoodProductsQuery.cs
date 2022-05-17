using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetFoodProductsQuery : IRequest<IEnumerable<FoodProductDto>>
    {
        public GetFoodProductsQuery()
        {
            
        }
    }
}
