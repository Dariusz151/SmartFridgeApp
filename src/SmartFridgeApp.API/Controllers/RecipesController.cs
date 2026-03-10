using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Application.Services;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.ValueObjects;

namespace SmartFridgeApp.API.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipesController(IRecipeService recipeService) : Controller
    {
        /// <summary>
        /// Get all available recipes.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecipeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRecipesAsync(CancellationToken ct)
        {
            return Ok(await recipeService.GetRecipesAsync(ct));
        }

        /// <summary>
        /// Add new recipe.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(Recipe), (int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRecipeAsync([FromBody] AddRecipeRequest request, CancellationToken ct)
        {
            var recipe = await recipeService.AddRecipeAsync(
                request.Name,
                request.Products,
                request.Description,
                request.RecipeCategory,
                request.RequiredTime,
                request.LevelOfDifficulty,
                ct);

            return Created(string.Empty, recipe);
        }

        /// <summary>
        /// Get list of matching recipes.
        /// </summary>
        [Route("find")]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Recipe>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindMatchingRecipesAsync([FromBody] FindRecipesRequest request, CancellationToken ct)
        {
            return Ok(await recipeService.FindRecipesAsync(request.FoodProducts, ct));
        }

        /// <summary>
        /// Update recipe details by given id.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRecipeAsync([FromBody] UpdateRecipeRequest request, CancellationToken ct)
        {
            await recipeService.UpdateRecipeAsync(
                request.RecipeId,
                request.Name,
                request.Description,
                request.Category,
                request.RequiredTime,
                request.LevelOfDifficulty,
                ct);

            return Ok();
        }

        /// <summary>
        /// Delete recipe by given id.
        /// </summary>
        [Route("")]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteRecipeAsync([FromBody] DeleteRecipeRequest request, CancellationToken ct)
        {
            await recipeService.DeleteRecipeAsync(request.RecipeId, ct);
            return NoContent();
        }

        /// <summary>
        /// Get all available recipe categories.
        /// </summary>
        [Route("/api/recipes/categories")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecipeCategory>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRecipeCategoriesAsync(CancellationToken ct)
        {
            return Ok(await recipeService.GetRecipeCategoriesAsync(ct));
        }

        /// <summary>
        /// Create new recipe category.
        /// </summary>
        [Route("/api/recipes/categories")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateRecipeCategoryAsync([FromBody] CreateRecipeCategoryRequest request, CancellationToken ct)
        {
            await recipeService.CreateRecipeCategoryAsync(request.Name, ct);
            return Created(string.Empty, null);
        }
    }
}
