using System.Collections.Generic;
using MediatR;

namespace SmartFridgeApp.API.FoodProducts.GetFoodProducts
{
    public class GetCategoriesQuery : IRequest<IEnumerable<FoodProductDto>>
    {
        public GetCategoriesQuery()
        {
            
        }
    }
}
