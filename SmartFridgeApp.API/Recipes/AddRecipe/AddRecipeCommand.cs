using System.Collections.Generic;
using MediatR;
using SmartFridgeApp.Domain.Models.Recipes;

namespace SmartFridgeApp.API.Recipes.AddRecipe
{
    public class AddRecipeCommand : IRequest<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<int> ProductIds { get; set; }

        public AddRecipeCommand(string name, List<int> productIds, string description, string category)
        {
            Name = name;
            ProductIds = productIds;
            Description = description;
            Category = category;
        }
    }
}
