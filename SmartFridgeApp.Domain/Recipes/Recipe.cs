using System.Collections.Generic;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Recipes
{
    public class Recipe : Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FoodProduct> Products { get; set; }

        public RecipeDetails Details { get; set; }
    }
}
