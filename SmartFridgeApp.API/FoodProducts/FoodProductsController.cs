using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.FoodProducts.AddFoodProduct;
using SmartFridgeApp.API.FoodProducts.Categories.CreateCategory;
using SmartFridgeApp.API.FoodProducts.Categories.GetCategories;
using SmartFridgeApp.API.FoodProducts.DeleteFoodProduct;
using SmartFridgeApp.API.FoodProducts.GetFoodProducts;
using SmartFridgeApp.API.FoodProducts.UpdateFoodProduct;
using SmartFridgeApp.Domain.Models.FoodProducts;

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
        [Route("/api/foodProducts/categories")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody]CreateCategoryRequest request)
        {
            await _mediator.Send(new CreateCategoryCommand(request.Name));
            
            return Ok();
        }

        /// <summary>
        /// Create new food product.
        /// </summary>
        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFoodProductAsync([FromBody]AddFoodProductRequest request)
        {
            await _mediator.Send(new AddFoodProductCommand(request.Name, request.Category));

            return Ok();
        }

        /// <summary>
        /// Update foodProduct name by given id.
        /// </summary>
        [Route("")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateFoodProductAsync([FromBody]UpdateFoodProductRequest request)
        {
            await _mediator.Send(new UpdateFoodProductCommand(request.FoodProductId, request.FoodProductName));

            return Ok();
        }

        /// <summary>
        /// Delete foodProduct by given id. Only if foodProduct isn't connected with any recipe.
        /// </summary>
        [Route("")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteFoodProductAsync([FromBody]DeleteFoodProductRequest request)
        {
            await _mediator.Send(new DeleteFoodProductCommand(request.FoodProductId));

            return NoContent();
        }
    }
}
