using System.Collections.Generic;

namespace SmartFridgeApp.Core.Application.Features.Recipes.ImportRecipes;

public class RecipeImportResult
{
    public int ImportedCount { get; set; }
    public int SkippedCount { get; set; }
    public List<string> NewFoodProducts { get; set; } = [];
    public List<string> ImportedRecipeNames { get; set; } = [];
    public List<string> Errors { get; set; } = [];
}
