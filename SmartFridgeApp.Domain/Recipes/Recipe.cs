using System;
using System.Collections.Generic;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.RecipeFoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.Recipes
{
    public class Recipe : Entity, IAggregateRoot
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        //public int DifficultyLevel { get; set; }
        //public int MinutesRequired { get; set; }
        //public string Category { get; set; }
        
        public List<RecipeFoodProduct> Products { get; set; }

        private Recipe()
        {
            // EF Core   
        }

        public Recipe(int recipeId, string name, List<RecipeFoodProduct> products)
        {
            RecipeId = recipeId;
            Name = name;
            Products = products;
        }

        //public Recipe(string name, string description, List<FoodProduct> products)
        //    : this(name, description, 0, 0, String.Empty, products)
        //{

        //}


        //public Recipe(string name, string description, int difficultyLevel, int minutesRequired, string category, List<FoodProduct> products)
        //{
        //    if (products.Count == 0)
        //        throw new DomainException("Recipe must have any products!");
        //    if (String.IsNullOrEmpty(name))
        //        throw new DomainException("Recipe has to have name!");
        //    Name = name;
        //    Description = description;
        //    DifficultyLevel = difficultyLevel;
        //    MinutesRequired = minutesRequired;
        //    Category = category;
        //    Products = products;
        //}
    }
}
