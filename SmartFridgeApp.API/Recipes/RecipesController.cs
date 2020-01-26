using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Recipes.AddRecipe;
using SmartFridgeApp.API.Recipes.DeleteRecipe;
using SmartFridgeApp.API.Recipes.GetRecipes;
using SmartFridgeApp.API.Recipes.UpdateRecipe;

namespace SmartFridgeApp.API.Recipes
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
        public async Task<IActionResult> GetAllRecipesAsync()
        {
            var recipes = await _mediator.Send(new GetRecipesQuery());

            return Ok(recipes);
        }

        /// <summary>
        /// Add new recipe.
        /// </summary>
        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddRecipeAsync([FromBody]AddRecipeRequest request)
        {
            var recipe = await _mediator.Send(new AddRecipeCommand(
                request.Name, 
                request.ProductIds,
                request.Description,
                request.DifficultyLevel,
                request.MinutesRequired,
                request.Category
                ));

            return Created(string.Empty, recipe);
        }

        /// <summary>
        /// Update recipe details by given id.
        /// </summary>
        [Route("")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRecipeAsync([FromBody]UpdateRecipeRequest request)
        {
            await _mediator.Send(new UpdateRecipeCommand(
                request.RecipeId,
                request.Name,
                request.Description,
                request.DifficultyLevel,
                request.MinutesRequired,
                request.Category
            ));

            return Ok();
        }

        /// <summary>
        /// Delete recipe by given id.
        /// </summary>
        [Route("")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteRecipeAsync([FromBody]DeleteRecipeRequest request)
        {
            await _mediator.Send(new DeleteRecipeCommand(request.RecipeId));

            return NoContent();
        }
    }
}
