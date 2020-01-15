using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.RecipeFoodProducts;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public List<RecipeFoodProduct> Products { get; set; }

        public AddRecipeCommand(string name, List<RecipeFoodProduct> products)
        {
            Name = name;
            Products = products;
        }
    }
}
