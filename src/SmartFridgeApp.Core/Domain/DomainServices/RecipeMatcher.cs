using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Domain.DomainServices;

public static class RecipeMatcher
{
    public static List<Recipe> FindAvailable(
        List<Recipe> recipes,
        HashSet<short> availableProductIds,
        List<short> selectedFoodProductIds)
    {
        var hasSelection = selectedFoodProductIds is { Count: > 0 };
        var selectedSet = hasSelection ? new HashSet<short>(selectedFoodProductIds) : null;

        var result = new List<Recipe>();
        foreach (var recipe in recipes)
        {
            var requiredIds = recipe.FoodProducts
                .Where(x => !x.IsOptional)
                .Select(x => x.FoodProductId)
                .ToList();

            if (hasSelection && !requiredIds.Any(id => selectedSet!.Contains(id)))
                continue;

            if (requiredIds.All(id => availableProductIds.Contains(id)))
                result.Add(recipe);
        }

        return result;
    }

    public static List<FoodProductDetails> FindMissing(
        Recipe recipe,
        HashSet<short> availableProductIds)
    {
        return recipe.FoodProducts
            .Where(x => !x.IsOptional && !availableProductIds.Contains(x.FoodProductId))
            .ToList();
    }
}
