using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        public int RequiredTime { get; set; }
        public LevelOfDifficulty LevelOfDifficulty { get; set; }
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
            : this(name, description, new RecipeCategory("Undefined"), products, 1, 0)
        {
        }
        
        public Recipe(string name, string description, RecipeCategory recipeCategory, List<FoodProductDetails> products, int requiredTime, int levelOfDifficulty)
        {
            if (products.Count == 0)
                throw new DomainException("Recipe must have any products!");
            if (String.IsNullOrEmpty(name))
                throw new DomainException("Recipe has to have name!");
            if (requiredTime <= 0)
                throw new DomainException("Recipe required time should be more than 0 minutes!");
            if (levelOfDifficulty < 0 || levelOfDifficulty > 3)
                throw new DomainException("Recipe levelOfDifficulty should have proper value!");
            RecipeId = Guid.NewGuid();
            Name = name;
            Description = description;
            RecipeCategory = recipeCategory;
            RequiredTime = requiredTime;
            LevelOfDifficulty = (LevelOfDifficulty)levelOfDifficulty;
            FoodProducts = products;

            //this.AddDomainEvent(new RecipeAddedEvent(this));
        }

        public void UpdateRecipeCategory(RecipeCategory recipeCategory)
        {
            if (string.IsNullOrEmpty(recipeCategory.Name))
                throw new DomainException("Cant update recipe with no name category!");
            RecipeCategory = recipeCategory;
        }

        public void UpdateRecipe(string name, string desc, RecipeCategory recipeCategory, int requiredTime, int levelOfDifficulty)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Recipe must have name!");
            if (requiredTime <= 0)
                throw new DomainException("Recipe required time should be more than 0 minutes!");
            if (levelOfDifficulty < 0 || levelOfDifficulty > 3)
                throw new DomainException("Recipe levelOfDifficulty should have proper value!");
           
            if (!string.IsNullOrEmpty(desc))
                Description = desc;

            Name = name;
            RequiredTime = requiredTime;
            LevelOfDifficulty = (LevelOfDifficulty)levelOfDifficulty;

            this.UpdateRecipeCategory(recipeCategory);

            //this.AddDomainEvent(new RecipeUpdatedEvent(this));
        }
    }
}
