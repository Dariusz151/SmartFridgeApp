using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.API.FoodProducts.GetFoodProducts
{
    public class GetFoodProductsQuery : IRequest<IEnumerable<FoodProductDto>>
    {
        public GetFoodProductsQuery()
        {
            
        }
    }
}
