using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.FoodProducts.AddFoodProduct;
using SmartFridgeApp.API.FoodProducts.Categories.CreateCategory;
using SmartFridgeApp.API.FoodProducts.Categories.GetCategories;
using SmartFridgeApp.API.FoodProducts.DeleteFoodProduct;
using SmartFridgeApp.API.FoodProducts.GetFoodProducts;
using SmartFridgeApp.API.FoodProducts.UpdateFoodProduct;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.API.FoodProducts
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
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());

            return Ok(categories);
        }

        /// <summary>
        /// Get all available foodProducts.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<FoodProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllFoodProductsAsync()
        {
            var foodProducts = await _mediator.Send(new GetFoodProductsQuery());

            return Ok(foodProducts);
        }

        /// <summary>
        /// Create new food product category.
        /// </summary>
        [Authorize]
        [Route("/api/foodProducts/categories")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody]CreateCategoryRequest request)
        {
            await _mediator.Send(new CreateCategoryCommand(request.Name));
            
            return Ok();
        }

        /// <summary>
        /// Create new food product.
        /// </summary>
        [Authorize]
        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddFoodProductAsync([FromBody]AddFoodProductRequest request)
        {
            try
            {
                await _mediator.Send(new AddFoodProductCommand(request.Name, request.Category));
            }
            catch (InvalidFoodProductCategoryException exception)
            {
                return BadRequest($"Cant create this product: {exception.Message}");
            }
            return Ok();
        }

        /// <summary>
        /// Update foodProduct name by given id.
        /// </summary>
        [Authorize]
        [Route("")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateFoodProductAsync([FromBody]UpdateFoodProductRequest request)
        {
            try
            {
                await _mediator.Send(new UpdateFoodProductCommand(request.FoodProductId, request.FoodProductName));
            }
            catch (FoodProductNotFoundException exception)
            {
                return BadRequest($"Cant update this product: {exception.Message}");
            }
            catch (DomainException domainException)
            {
                return BadRequest($"Cant update this product: {domainException.Message}");
            }
            
            return Ok();
        }

        /// <summary>
        /// Delete foodProduct by given id. Only if foodProduct isn't connected with any recipe.
        /// </summary>
        [Authorize]
        [Route("")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> DeleteFoodProductAsync([FromBody]DeleteFoodProductRequest request)
        {
            try
            {
                await _mediator.Send(new DeleteFoodProductCommand(request.FoodProductId));
            }
            catch (FoodProductNotFoundException exception)
            {
                return BadRequest($"Cant delete this product: {exception.Message}");
            }

            return NoContent();
        }
    }
}
