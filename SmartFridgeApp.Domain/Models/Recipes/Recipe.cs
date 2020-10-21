using System;
using System.Collections.Generic;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.Domain.Models.Recipes
{
    public class Recipe : Entity, IAggregateRoot
    {
        public Guid RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RecipeCategory RecipeCategory { get; set; }

        public List<FoodProductDetails> FoodProducts { get; set; }
        
        private Recipe()
        {
            // EF Core   
        }

        public Recipe(string name, List<FoodProductDetails> products)
            : this(name, String.Empty, products)
        {
        }

        public Recipe(string name, string description, List<FoodProductDetails> products)
            : this(name, description, new RecipeCategory("Undefined"), products)
        {
        }
        
        public Recipe(string name, string description, RecipeCategory recipeCategory, List<FoodProductDetails> products)
        {
            if (products.Count == 0)
                throw new DomainException("Recipe must have any products!");
            if (String.IsNullOrEmpty(name))
                throw new DomainException("Recipe has to have name!");
            RecipeId = Guid.NewGuid();
            Name = name;
            Description = description;
            RecipeCategory = recipeCategory;
            FoodProducts = products;

            //this.AddDomainEvent(new RecipeAddedEvent(this));
        }

        public void UpdateRecipe(string name, string desc, RecipeCategory recipeCategory)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Recipe must have name!");
            Name = name;

            if (!string.IsNullOrEmpty(desc))
                Description = desc;
            
            if (!string.IsNullOrEmpty(recipeCategory.Name))
                RecipeCategory = recipeCategory;

            //this.AddDomainEvent(new RecipeUpdatedEvent(this));
        }
    }
}
