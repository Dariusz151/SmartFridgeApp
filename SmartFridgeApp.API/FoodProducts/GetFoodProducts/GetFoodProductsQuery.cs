using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;

namespace SmartFridgeApp.API.FoodProducts.GetFoodProducts
{
    public class GetFoodProductsQuery : IRequest<IEnumerable<FoodProductDto>>
    {
        public GetFoodProductsQuery()
        {
            
        }
    }
}
