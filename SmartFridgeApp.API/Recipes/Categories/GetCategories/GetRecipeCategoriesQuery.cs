using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.API.Recipes.Categories.GetCategories
{
    public class GetRecipeCategoriesQuery : IRequest<IEnumerable<RecipeCategory>>
    {
        public GetRecipeCategoriesQuery()
        {
            
        }
    }
}
