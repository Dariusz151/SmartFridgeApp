using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features.Recipes.ImportRecipes;
using SmartFridgeApp.Core.Application.Services;

namespace SmartFridgeApp.API.Controllers;

[Route("api/recipes/import")]
[ApiController]
public class RecipeImportController(IRecipeImportService recipeImportService) : Controller
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(RecipeImportResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportRecipesAsync(
        [FromQuery] int batchSize = 5,
        CancellationToken ct = default)
    {
        if (batchSize is < 1 or > 100)
            return BadRequest("batchSize must be between 1 and 100.");

        var result = await recipeImportService.ImportRecipesAsync(batchSize, ct);
        return Ok(result);
    }
}
