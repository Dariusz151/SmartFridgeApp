using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Contracts.DomainServices;

public interface IRecipeFinderService
{
    Task<List<Recipe>> FindAvailableRecipes(Guid kitchenId, List<short> selectedFoodProductIds);
    Task<List<FoodProductDetails>> GetMissingProducts(Guid kitchenId, Guid recipeId);
}
