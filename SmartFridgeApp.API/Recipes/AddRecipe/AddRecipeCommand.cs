using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public List<int> ProductIds { get; set; }

        public AddRecipeCommand(string name, List<int> productIds)
        {
            Name = name;
            ProductIds = productIds;
        }
    }
}
