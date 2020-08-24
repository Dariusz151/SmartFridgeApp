﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly SmartFridgeAppContext _context;

        public RecipeRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task<Recipe> GetRecipeByIdAsync(Guid recipeId)
        {
            try
            {
                var recipe = await _context.Recipes.SingleAsync(x => x.RecipeId == recipeId);
                return recipe;
            }
            catch
            {
                throw new DomainException("This recipe does not exist.");
            }
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
        }

        public async Task DeleteRecipeAsync(Guid recipeId)
        {
            var recipe = await GetRecipeByIdAsync(recipeId);

            _context.Recipes.Remove(recipe);
        }
    }
}
