using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.Infrastructure.Recipes
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly SmartFridgeAppContext _context;

        public RecipeRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
        }
    }
}
