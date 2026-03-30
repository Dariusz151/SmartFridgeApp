using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features.Recipes.ImportRecipes;

namespace SmartFridgeApp.Core.Application.Services;

public interface IRecipeImportService
{
    Task<RecipeImportResult> ImportRecipesAsync(int batchSize, CancellationToken ct = default);
}
