using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features;

public class FindRecipesRequest
{
    public List<short> SelectedFoodProductIds { get; set; } = [];
}

public record MissingProductDto(short FoodProductId, string FoodProductName);

public record AddMissingToShoppingListRequest(Guid RecipeId);
