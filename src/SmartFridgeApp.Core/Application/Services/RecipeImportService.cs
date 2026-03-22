using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features.Recipes.ImportRecipes;
using SmartFridgeApp.Core.Contracts.ExternalRecipes;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class RecipeImportService(
    IExternalRecipeSource externalRecipeSource,
    IFoodProductRepository foodProductRepository,
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork) : IRecipeImportService
{
    private static readonly Dictionary<string, int> DishTypeToCategoryId = new(StringComparer.OrdinalIgnoreCase)
    {
        ["breakfast"] = 1,
        ["morning meal"] = 1,
        ["lunch"] = 2,
        ["main course"] = 2,
        ["main dish"] = 2,
        ["dinner"] = 2,
        ["supper"] = 3,
        ["drink"] = 4,
        ["beverage"] = 4,
        ["snack"] = 5,
        ["appetizer"] = 5,
        ["starter"] = 5,
        ["antipasti"] = 5,
        ["antipasto"] = 5,
        ["hor d'oeuvre"] = 5,
        ["dessert"] = 6,
        ["soup"] = 7,
    };

    private static readonly Dictionary<string, Unit> UnitMapping = new(StringComparer.OrdinalIgnoreCase)
    {
        ["g"] = Unit.Grams,
        ["grams"] = Unit.Grams,
        ["gram"] = Unit.Grams,
        ["kg"] = Unit.Grams,
        ["ml"] = Unit.Mililiter,
        ["milliliters"] = Unit.Mililiter,
        ["milliliter"] = Unit.Mililiter,
        ["l"] = Unit.Mililiter,
        ["liter"] = Unit.Mililiter,
        ["liters"] = Unit.Mililiter,
    };

    private const int DefaultRecipeCategoryId = 2;
    private const int DefaultFoodProductCategoryId = 12;
    private const int FoodProductNameMaxLength = 40;

    public async Task<RecipeImportResult> ImportRecipesAsync(int batchSize, CancellationToken ct = default)
    {
        var result = new RecipeImportResult();

        var externalRecipes = await externalRecipeSource.FetchRecipesAsync(batchSize, ct);
        
        Console.WriteLine(externalRecipes.Count);

        var existingFoodProducts = (await foodProductRepository.GetAllAsync()).ToList();
        var existingRecipes = await recipeRepository.GetAllRecipesAsync();
        var existingRecipeNames = new HashSet<string>(
            existingRecipes.Select(r => r.Name), StringComparer.OrdinalIgnoreCase);

        var inneCategory = await foodProductRepository.GetCategoryByIdAsync(DefaultFoodProductCategoryId);
        var recipeCategories = (await recipeRepository.GetAllRecipeCategoriesAsync()).ToList();

        var newFoodProducts = new List<FoodProduct>();

        foreach (var external in externalRecipes)
        {
            try
            {
                Console.WriteLine(external.Title);
                if (existingRecipeNames.Contains(external.Title))
                {
                    result.SkippedCount++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(external.Title) || external.Ingredients.Count == 0)
                {
                    result.SkippedCount++;
                    continue;
                }

                var foodProductDetailsList = new List<FoodProductDetails>();
                foreach (var ingredient in external.Ingredients)
                {
                    var (foodProduct, isNew) = MatchOrCreateFoodProduct(
                        ingredient.Name, existingFoodProducts, newFoodProducts, inneCategory);

                    if (isNew)
                    {
                        Console.WriteLine("Adding new ingredient");
                        await foodProductRepository.AddAsync(foodProduct);
                        newFoodProducts.Add(foodProduct);
                        result.NewFoodProducts.Add(foodProduct.Name);
                    }

                    var (amount, unit) = MapAmountAndUnit(ingredient.Amount, ingredient.Unit);
                    var amountValue = new AmountValue(amount, unit);
                    foodProductDetailsList.Add(new FoodProductDetails(
                        foodProduct.FoodProductId, foodProduct.Name, amountValue));
                }

                if (foodProductDetailsList.Count == 0)
                {
                    result.SkippedCount++;
                    continue;
                }

                // Commit new FoodProducts so auto-generated IDs are assigned
                if (newFoodProducts.Count > 0)
                {
                    await unitOfWork.CommitAsync(ct);
                    foodProductDetailsList = RebuildDetailsWithIds(
                        foodProductDetailsList, existingFoodProducts, newFoodProducts);
                }

                var recipeCategoryId = MapRecipeCategory(external.DishTypes);
                var recipeCategory = recipeCategories.FirstOrDefault(c => c.RecipeCategoryId == recipeCategoryId)
                                     ?? recipeCategories.First(c => c.RecipeCategoryId == DefaultRecipeCategoryId);

                var difficulty = MapDifficulty(external.ReadyInMinutes);

                var recipeName = external.Title.Length > 100
                    ? external.Title[..100]
                    : external.Title;

                var recipe = new Recipe(
                    recipeName,
                    external.Description ?? string.Empty,
                    recipeCategory,
                    foodProductDetailsList,
                    external.ReadyInMinutes > 0 ? external.ReadyInMinutes : 1,
                    (int)difficulty);
                
                Console.WriteLine($"Adding new recipe: {recipe.Name} ");

                await recipeRepository.AddRecipeAsync(recipe);
                existingRecipeNames.Add(recipeName);
                result.ImportedRecipeNames.Add(recipeName);
                result.ImportedCount++;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to import '{external.Title}': {ex.Message}");
                result.SkippedCount++;
            }
        }

        await unitOfWork.CommitAsync(ct);

        return result;
    }

    private static (FoodProduct product, bool isNew) MatchOrCreateFoodProduct(
        string ingredientName,
        List<FoodProduct> existing,
        List<FoodProduct> newlyCreated,
        Category defaultCategory)
    {
        var trimmed = ingredientName.Trim();
        if (trimmed.Length > FoodProductNameMaxLength)
            trimmed = trimmed[..FoodProductNameMaxLength];

        // Exact match
        var match = existing.FirstOrDefault(fp =>
            fp.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
        if (match is not null) return (match, false);

        match = newlyCreated.FirstOrDefault(fp =>
            fp.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
        if (match is not null) return (match, false);

        // Contains match
        match = existing.FirstOrDefault(fp =>
            fp.Name.Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
            trimmed.Contains(fp.Name, StringComparison.OrdinalIgnoreCase));
        if (match is not null) return (match, false);

        match = newlyCreated.FirstOrDefault(fp =>
            fp.Name.Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
            trimmed.Contains(fp.Name, StringComparison.OrdinalIgnoreCase));
        if (match is not null) return (match, false);

        // Auto-create
        var newProduct = new FoodProduct(trimmed, defaultCategory);
        return (newProduct, true);
    }

    private static List<FoodProductDetails> RebuildDetailsWithIds(
        List<FoodProductDetails> original,
        List<FoodProduct> existing,
        List<FoodProduct> newlyCreated)
    {
        var allProducts = existing.Concat(newlyCreated).ToList();
        return original.Select(d =>
        {
            var fp = allProducts.FirstOrDefault(p =>
                p.Name.Equals(d.FoodProductName, StringComparison.OrdinalIgnoreCase));
            var id = fp?.FoodProductId ?? d.FoodProductId;
            return new FoodProductDetails(id, d.FoodProductName, d.AmountValue);
        }).ToList();
    }

    private static (float amount, Unit unit) MapAmountAndUnit(float rawAmount, string rawUnit)
    {
        var amount = rawAmount > 0 ? rawAmount : 1;
        var unit = Unit.NotAssigned;

        if (!string.IsNullOrWhiteSpace(rawUnit) && UnitMapping.TryGetValue(rawUnit, out var mapped))
        {
            unit = mapped;
            if (rawUnit.Equals("kg", StringComparison.OrdinalIgnoreCase))
                amount *= 1000;
            else if (rawUnit.Equals("l", StringComparison.OrdinalIgnoreCase) ||
                     rawUnit.Equals("liter", StringComparison.OrdinalIgnoreCase) ||
                     rawUnit.Equals("liters", StringComparison.OrdinalIgnoreCase))
                amount *= 1000;
        }
        else if (!string.IsNullOrWhiteSpace(rawUnit))
        {
            if (amount == MathF.Floor(amount) && amount <= 50)
                unit = Unit.Pieces;
        }

        return (amount, unit);
    }

    private static int MapRecipeCategory(List<string> dishTypes)
    {
        if (dishTypes is null || dishTypes.Count == 0)
            return DefaultRecipeCategoryId;

        foreach (var dt in dishTypes)
        {
            if (DishTypeToCategoryId.TryGetValue(dt, out var categoryId))
                return categoryId;
        }

        return DefaultRecipeCategoryId;
    }

    private static LevelOfDifficulty MapDifficulty(int readyInMinutes) => readyInMinutes switch
    {
        <= 15 => LevelOfDifficulty.Easy,
        <= 45 => LevelOfDifficulty.Medium,
        _ => LevelOfDifficulty.Hard,
    };
}
