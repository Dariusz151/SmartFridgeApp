using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.API.Recipes.FindRecipes
{
    public class FindRecipesCommand : IRequest<IEnumerable<Recipe>>
    {
        public List<short> FoodProducts { get; set; }

        public FindRecipesCommand(List<short> foodProducts)
        {
            this.FoodProducts = foodProducts;
        }
    }
}
