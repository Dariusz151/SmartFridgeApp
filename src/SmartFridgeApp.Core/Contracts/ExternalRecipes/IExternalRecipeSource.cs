using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts.ExternalRecipes;

public interface IExternalRecipeSource
{
    Task<List<ExternalRecipe>> FetchRecipesAsync(int batchSize, CancellationToken ct = default);
}
