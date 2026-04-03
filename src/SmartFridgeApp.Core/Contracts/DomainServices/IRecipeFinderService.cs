using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Contracts.DomainServices;

public interface IRecipeFinderService
{
    Task<List<Recipe>> FindAvailableRecipes(Guid kitchenId, List<short> selectedFoodProductIds, int? memberId = null);
    Task<List<FoodProductDetails>> GetMissingProducts(Guid kitchenId, Guid recipeId, int? memberId = null);
}
