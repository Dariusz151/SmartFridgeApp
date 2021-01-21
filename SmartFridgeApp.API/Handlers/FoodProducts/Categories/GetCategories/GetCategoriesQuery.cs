using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;

namespace SmartFridgeApp.API.FoodProducts.Categories.GetCategories
{
    public class GetCategoriesQuery : IRequest<IEnumerable<Category>>
    {
        public GetCategoriesQuery()
        {
            
        }
    }
}
