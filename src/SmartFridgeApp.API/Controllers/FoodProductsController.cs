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
    [Route("api/foodProducts")]
    [ApiController]
    public class FoodProductsController(IFoodProductService foodProductService) : Controller
    {
        /// <summary>
        /// Get all available categories for foodProducts.
        /// </summary>
        [Route("/api/foodProducts/categories")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> GetAllCategoriesAsync(CancellationToken ct)
        {
            return Ok(await foodProductService.GetCategoriesAsync(ct));
        }

        /// <summary>
        /// Get all available foodProducts.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FoodProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoodProductsAsync(CancellationToken ct)
        {
            return Ok(await foodProductService.GetFoodProductsAsync(ct));
        }

        /// <summary>
        /// Create new food product category.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [Route("/api/foodProducts/categories")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            await foodProductService.CreateCategoryAsync(request.Name, ct);
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Create new food product.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddFoodProductAsync([FromBody] AddFoodProductRequest request, CancellationToken ct)
        {
            await foodProductService.AddFoodProductAsync(request.Name, request.Category, ct);
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Update foodProduct name by given id.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateFoodProductAsync([FromBody] UpdateFoodProductRequest request, CancellationToken ct)
        {
            await foodProductService.UpdateFoodProductAsync(request.FoodProductId, request.FoodProductName, ct);
            return Ok();
        }

        /// <summary>
        /// Delete foodProduct by given id. Only if foodProduct isn't connected with any recipe.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFoodProductAsync([FromBody] DeleteFoodProductRequest request, CancellationToken ct)
        {
            await foodProductService.DeleteFoodProductAsync(request.FoodProductId, ct);
            return NoContent();
        }

        /// <summary>
        /// Get variants for a food product.
        /// </summary>
        [Route("{foodProductId:int}/variants")]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductVariantDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVariantsAsync([FromRoute] short foodProductId, CancellationToken ct)
        {
            return Ok(await foodProductService.GetVariantsAsync(foodProductId, ct));
        }

        /// <summary>
        /// Add a variant to a food product.
        /// </summary>
        [Route("{foodProductId:int}/variants")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddVariantAsync([FromRoute] short foodProductId, [FromBody] AddVariantRequest request, CancellationToken ct)
        {
            await foodProductService.AddVariantAsync(foodProductId, request.Name, request.Barcode, ct);
            return Created(string.Empty, null);
        }
    }
}
