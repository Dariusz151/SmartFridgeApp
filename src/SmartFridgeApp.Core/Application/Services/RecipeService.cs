using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.DomainServices;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Core.Extensions;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class RecipeService(
    IRecipeRepository recipeRepository,
    IFoodProductRepository foodProductRepository,
    IRecipeFinderService recipeFinderService,
    IShoppingListService shoppingListService,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IRecipeService
{
    public async Task<IEnumerable<RecipeDto>> GetRecipesAsync(CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT r."RecipeId",
                   r."Name" AS "RecipeName",
                   r."Description",
                   rc."Name" AS "RecipeCategory",
                   r."FoodProducts",
                   r."RequiredTime",
                   r."LevelOfDifficulty" AS "LevelOfDifficultyId"
            FROM app."Recipes" r
            LEFT JOIN app."RecipeCategories" rc ON r."RecipeCategoryId" = rc."RecipeCategoryId"
            """;

        var recipes = await connection.QueryAsync<RecipeDto>(sql);
        foreach (RecipeDto recipe in (IList)recipes)
        {
            recipe.FoodProducts = ConvertHelper.ConvertXmlToJson(recipe.FoodProducts);
            recipe.LevelOfDifficulty = Enum.GetName(typeof(LevelOfDifficulty), recipe.LevelOfDifficultyId);
        }

        return recipes;
    }

    public async Task<Recipe> AddRecipeAsync(
        string name,
        List<FoodProductDetailsDto> products,
        string description,
        int recipeCategory,
        int requiredTime,
        int levelOfDifficulty,
        CancellationToken ct = default)
    {
        var allFoodProducts = await foodProductRepository.GetAllAsync();
        List<short> allFoodProductsId = allFoodProducts.Select(x => x.FoodProductId).ToList();
        List<short> insertedFoodProductsId = products.Select(x => x.FoodProductId).ToList();

        if (!allFoodProductsId.ContainsAllItems(insertedFoodProductsId))
        {
            throw new DomainException("DomainException", "Some product id's does not exist in database.");
        }

        List<FoodProductDetails> productsDetails = new List<FoodProductDetails>();
        foreach (var item in products)
        {
            var foodProductName = allFoodProducts.SingleOrDefault(x => x.FoodProductId == item.FoodProductId)?.Name;
            var fpd = new FoodProductDetails(item.FoodProductId, foodProductName, item.AmountValue, item.IsOptional);
            productsDetails.Add(fpd);
        }

        var recipeCategoryEntity = await recipeRepository.GetRecipeCategoryByIdAsync(recipeCategory);

        var recipe = new Recipe(
            name,
            description,
            recipeCategoryEntity,
            productsDetails,
            requiredTime,
            levelOfDifficulty);

        await recipeRepository.AddRecipeAsync(recipe);
        await unitOfWork.CommitAsync(ct);

        return recipe;
    }

    public async Task UpdateRecipeAsync(Guid recipeId, string name, string description, int recipeCategory, int requiredTime, int levelOfDifficulty, CancellationToken ct = default)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(recipeId);
        var newRecipeCategory = await recipeRepository.GetRecipeCategoryByIdAsync(recipeCategory);

        recipe.UpdateRecipe(name, description, newRecipeCategory, requiredTime, levelOfDifficulty);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task DeleteRecipeAsync(Guid recipeId, CancellationToken ct = default)
    {
        await recipeRepository.DeleteRecipeAsync(recipeId);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<IEnumerable<Recipe>> FindRecipesForKitchenAsync(Guid kitchenId, List<short> selectedFoodProductIds, int? memberId = null, CancellationToken ct = default)
    {
        var recipes = await recipeFinderService.FindAvailableRecipes(kitchenId, selectedFoodProductIds, memberId);
        return recipes.AsEnumerable();
    }

    public async Task<List<FoodProductDetails>> GetMissingProductsAsync(Guid kitchenId, Guid recipeId, int? memberId = null, CancellationToken ct = default)
    {
        return await recipeFinderService.GetMissingProducts(kitchenId, recipeId, memberId);
    }

    public async Task AddMissingProductsToShoppingListAsync(Guid kitchenId, Guid recipeId, string userEmail, CancellationToken ct = default)
    {
        var missingProducts = await recipeFinderService.GetMissingProducts(kitchenId, recipeId);
        if (missingProducts.Count == 0) return;

        var productNames = missingProducts.Select(p => p.FoodProductName).ToList();
        await shoppingListService.AddItemsAsync(kitchenId, productNames, userEmail, ct);
    }

    public async Task<IEnumerable<RecipeCategory>> GetRecipeCategoriesAsync(CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "RecipeCategoryId", "Name"
            FROM app."RecipeCategories"
            """;

        var categories = await connection.QueryAsync<RecipeCategory>(sql);
        return categories.AsEnumerable();
    }

    public async Task CreateRecipeCategoryAsync(string name, CancellationToken ct = default)
    {
        var category = new RecipeCategory(name);
        await recipeRepository.CreateRecipeCategoryAsync(category);
        await unitOfWork.CommitAsync(ct);
    }
}
