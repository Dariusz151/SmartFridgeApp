using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.FoodProducts;

namespace SmartFridgeApp.API.FoodProducts.GetFoodProducts
{
    public class GetFoodProductsQuery : IRequest<List<FoodProduct>>
    {
        public GetFoodProductsQuery()
        {
            
        }
    }
}
