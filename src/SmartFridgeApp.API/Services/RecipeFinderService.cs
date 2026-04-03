using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.DomainServices;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.API.Services;

public class RecipeFinderService(
    IRecipeRepository recipeRepository,
    IKitchenInventoryRepository inventoryRepository) : IRecipeFinderService
{
    public async Task<List<Recipe>> FindAvailableRecipes(Guid kitchenId, List<short> selectedFoodProductIds, int? memberId = null)
    {
        var activeItems = memberId.HasValue
            ? await inventoryRepository.GetActiveItemsByMemberAsync(kitchenId, memberId.Value)
            : await inventoryRepository.GetActiveItemsByKitchenAsync(kitchenId);
        var availableProductIds = new HashSet<short>(activeItems.Select(x => x.FoodProductId));
        var recipes = await recipeRepository.GetAllRecipesAsync();

        return RecipeMatcher.FindAvailable(recipes, availableProductIds, selectedFoodProductIds);
    }

    public async Task<List<FoodProductDetails>> GetMissingProducts(Guid kitchenId, Guid recipeId, int? memberId = null)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(recipeId);
        var activeItems = memberId.HasValue
            ? await inventoryRepository.GetActiveItemsByMemberAsync(kitchenId, memberId.Value)
            : await inventoryRepository.GetActiveItemsByKitchenAsync(kitchenId);
        var availableProductIds = new HashSet<short>(activeItems.Select(x => x.FoodProductId));

        return RecipeMatcher.FindMissing(recipe, availableProductIds);
    }
}
