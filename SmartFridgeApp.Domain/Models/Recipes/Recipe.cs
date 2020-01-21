﻿using System;
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
        public int DifficultyLevel { get; set; }
        public int MinutesRequired { get; set; }
        public string Category { get; set; }

        public ICollection<RecipeFoodProduct> RecipeFoodProducts { get; set; }

        private Recipe()
        {
            // EF Core   
        }

        public Recipe(string name, ICollection<RecipeFoodProduct> products)
            : this(name, String.Empty, products)
        {
        }

        public Recipe(string name, string description, ICollection<RecipeFoodProduct> products)
            : this(name, description, 0, 0, String.Empty, products)
        {
        }
        
        public Recipe(string name, string description, int difficultyLevel, int minutesRequired, string category, ICollection<RecipeFoodProduct> products)
        {
            if (products.Count == 0)
                throw new DomainException("Recipe must have any products!");
            if (String.IsNullOrEmpty(name))
                throw new DomainException("Recipe has to have name!");
            Name = name;
            Description = description;
            DifficultyLevel = difficultyLevel;
            MinutesRequired = minutesRequired;
            Category = category;
            RecipeFoodProducts = products;

            this.AddDomainEvent(new RecipeAddedEvent(this));
        }
    }
}
