using System;
using System.Collections.Generic;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.Domain.Models.Recipes
{
    public class Recipe : Entity, IAggregateRoot
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<FoodProduct> FoodProducts { get; set; }
        
        private Recipe()
        {
            // EF Core   
        }

        public Recipe(string name, List<FoodProduct> products)
            : this(name, String.Empty, products)
        {
        }

        public Recipe(string name, string description, List<FoodProduct> products)
            : this(name, description, String.Empty, products)
        {
        }
        
        public Recipe(string name, string description, string type, List<FoodProduct> products)
        {
            if (products.Count == 0)
                throw new DomainException("Recipe must have any products!");
            if (String.IsNullOrEmpty(name))
                throw new DomainException("Recipe has to have name!");
            Name = name;
            Description = description;
            Type = type;
            FoodProducts = products;

            this.AddDomainEvent(new RecipeAddedEvent(this));
        }

        public void UpdateRecipe(string name, string desc, string type)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Recipe must have name!");
            Name = name;

            if (!string.IsNullOrEmpty(desc))
                Description = desc;
            
            if (!string.IsNullOrEmpty(type))
                Type = type;

            this.AddDomainEvent(new RecipeUpdatedEvent(this));
        }
    }
}
