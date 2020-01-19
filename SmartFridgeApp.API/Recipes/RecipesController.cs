using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.API.Fridges.AddFridge;
using SmartFridgeApp.API.Fridges.GetFridges;
using SmartFridgeApp.API.Recipes.AddRecipe;
using SmartFridgeApp.Domain.Recipes;

namespace SmartFridgeApp.API.Recipes
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipesController : Controller
    {
            private readonly IMediator _mediator;
            // TODO: Delete repository from here.
            private readonly IRecipeRepository _recipeRepository;

            public RecipesController(IMediator mediator, IRecipeRepository recipeRepository)
            {
                _mediator = mediator;
                _recipeRepository = recipeRepository;
            }

        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddRecipeAsync([FromBody]AddRecipeRequest request)
        {
            // TODO: Test if recipe add data to RecipeFoodProduct

            var recipe = await _mediator.Send(new AddRecipeCommand(request.Name, request.ProductIds));

            return Created(string.Empty, recipe);
        }
    }
}
