using System.Collections.Generic;

namespace SmartFridgeApp.Core.Contracts.ExternalRecipes;

public class ExternalRecipe
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadyInMinutes { get; set; }
    public List<string> DishTypes { get; set; } = [];
    public List<ExternalIngredient> Ingredients { get; set; } = [];
}

public class ExternalIngredient
{
    public string Name { get; set; }
    public float Amount { get; set; }
    public string Unit { get; set; }
}
