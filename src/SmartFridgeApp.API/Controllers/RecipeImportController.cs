using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartFridgeApp.API.Controllers;

[Route("api/recipes/import")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class RecipeImportController : Controller
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult ImportRecipesAsync()
    {
        return StatusCode(503, "Recipe import is currently disabled.");
    }
}
