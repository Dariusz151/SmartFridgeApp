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
    [Route("api/foodProducts")]
    [ApiController]
    public class FoodProductsController : Controller
    {
        private readonly IMediator _mediator;

        public FoodProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all available categories for foodProducts.
        /// </summary>
        [Route("/api/foodProducts/categories")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return Ok(await _mediator.Send(new GetCategoriesQuery()));
        }

        /// <summary>
        /// Get all available foodProducts.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FoodProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoodProductsAsync()
        {
            return Ok(await _mediator.Send(new GetFoodProductsQuery()));
        }

        /// <summary>
        /// Create new food product category.
        /// </summary>
        [Authorize]
        [Route("/api/foodProducts/categories")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody]CreateCategoryRequest request)
        {
            await _mediator.Send(new CreateCategoryCommand(request.Name));
            
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Create new food product.
        /// </summary>
        [Authorize]
        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddFoodProductAsync([FromBody]AddFoodProductRequest request)
        {
            await _mediator.Send(new AddFoodProductCommand(request.Name, request.Category));
            
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Update foodProduct name by given id.
        /// </summary>
        [Authorize]
        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateFoodProductAsync([FromBody]UpdateFoodProductRequest request)
        {
            await _mediator.Send(new UpdateFoodProductCommand(request.FoodProductId, request.FoodProductName));
            
            return Ok();
        }

        /// <summary>
        /// Delete foodProduct by given id. Only if foodProduct isn't connected with any recipe.
        /// </summary>
        [Authorize]
        [Route("")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFoodProductAsync([FromBody]DeleteFoodProductRequest request)
        {
            await _mediator.Send(new DeleteFoodProductCommand(request.FoodProductId));
            
            return NoContent();
        }
    }
}
