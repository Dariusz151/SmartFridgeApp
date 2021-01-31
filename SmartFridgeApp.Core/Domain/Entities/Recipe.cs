using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Domain.Entities
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

        }

        public Recipe(string name, List<FoodProductDetails> products)
            : this(name, String.Empty, products)
        {
        }

        public Recipe(string name, string description, List<FoodProductDetails> products)
            : this(name, description, new RecipeCategory("Undefined"), products, 1, (int)LevelOfDifficulty.Unknown)
        {
        }

        public Recipe(string name, string description, RecipeCategory recipeCategory, List<FoodProductDetails> products, int requiredTime, int levelOfDifficulty)
        {
            ValidateRecipeName(name);
            ValidateRequiredTime(requiredTime);

            if (products.Count == 0)
                throw new DomainException("Recipe must have any products!", "InvalidRecipeProducts");
            if (levelOfDifficulty < (int)LevelOfDifficulty.Easy || levelOfDifficulty > (int)LevelOfDifficulty.Unknown)
                throw new DomainException("Recipe levelOfDifficulty should have proper value!", "InvalidDifficultyLevel");
            RecipeId = Guid.NewGuid();
            Name = name;
            Description = description;
            RecipeCategory = recipeCategory;
            RequiredTime = requiredTime;
            LevelOfDifficulty = (LevelOfDifficulty)levelOfDifficulty;
            FoodProducts = products;

            this.AddDomainEvent(new RecipeAddedEvent(this));
        }

        public void UpdateRecipe(string name, string desc, RecipeCategory recipeCategory, int requiredTime, int levelOfDifficulty)
        {
            ValidateRecipeName(name);
            ValidateRequiredTime(requiredTime);
            if (levelOfDifficulty < 0 || levelOfDifficulty > 3)
                throw new DomainException("Recipe levelOfDifficulty should have proper value!");
            if (!string.IsNullOrEmpty(desc))
                Description = desc;

            Name = name;
            RequiredTime = requiredTime;
            LevelOfDifficulty = (LevelOfDifficulty)levelOfDifficulty;

            this.UpdateRecipeCategory(recipeCategory);
        }

        public void UpdateRecipeCategory(RecipeCategory recipeCategory)
        {
            if (string.IsNullOrEmpty(recipeCategory.Name))
                throw new DomainException("Cant update recipe with no name category!", "InvalidRecipeCategory");
            RecipeCategory = recipeCategory;
        }

        private void ValidateRequiredTime(int val)
        {
            if (val <= 0)
                throw new InvalidInputException("Recipe required time should be more than 0 minutes!", "InvalidRequiredTime");
        }

        private void ValidateRecipeName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new InvalidInputException("Recipe has to have name!", "InvalidRecipeName");
        }
    }
}