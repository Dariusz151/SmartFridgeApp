using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RecipeCategory { get; set; }

        public List<FoodProductDetails> Products { get; set; }
        //public List<int> ProductIds { get; set; }

        public AddRecipeCommand(string name, List<FoodProductDetails> products, string description, string category)
        {
            Name = name;
            Products = products;
            Description = description;
            RecipeCategory = category;
        }
    }
}
