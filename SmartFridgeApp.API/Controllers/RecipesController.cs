using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.API.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipesController : Controller
    {
        private readonly IMediator _mediator;
       
        public RecipesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all available recipes.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecipeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRecipesAsync()
        {
            return Ok(await _mediator.Send(new GetRecipesQuery()));
        }

        /// <summary>
        /// Add new recipe.
        /// </summary>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(Recipe), (int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRecipeAsync([FromBody]AddRecipeRequest request)
        {
            var recipe = await _mediator.Send(new AddRecipeCommand(
                request.Name, 
                request.Products,
                request.Description,
                request.RecipeCategory,
                request.RequiredTime,
                request.LevelOfDifficulty
                ));

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
        public async Task<IActionResult> FindMatchingRecipesAsync([FromBody]FindRecipesRequest request)
        {
            return Ok(await _mediator.Send(new FindRecipesCommand(request.FoodProducts)));
        }

        /// <summary>
        /// Update recipe details by given id.
        /// </summary>
        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRecipeAsync([FromBody]UpdateRecipeRequest request)
        {
            await _mediator.Send(new UpdateRecipeCommand(
                request.RecipeId,
                request.Name,
                request.Description,
                request.Category,
                request.RequiredTime,
                request.LevelOfDifficulty
            ));

            return Ok();
        }

        /// <summary>
        /// Delete recipe by given id.
        /// </summary>
        [Route("")]
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteRecipeAsync([FromBody]DeleteRecipeRequest request)
        {
            await _mediator.Send(new DeleteRecipeCommand(request.RecipeId));

            return NoContent();
        }

        /// <summary>
        /// Get all available recipe categories .
        /// </summary>
        [Route("/api/recipes/categories")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecipeCategory>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRecipeCategoriesAsync()
        {
            return Ok(await _mediator.Send(new GetRecipeCategoriesQuery()));
        }

        /// <summary>
        /// Create new recipe category.
        /// </summary>
        [Route("/api/recipes/categories")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateRecipeCategoryAsync([FromBody]CreateRecipeCategoryRequest request)
        {
            await _mediator.Send(new CreateRecipeCategoryCommand(request.Name));

            return Created(string.Empty, null);
        }
    }
}
