using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Application.Services;

namespace SmartFridgeApp.API.Controllers
{
    [Route("api/fridgeItems")]
    [ApiController]
    [Authorize]
    public class FridgeItemsController(IFridgeItemService fridgeItemService) : Controller
    {
        [Route("{fridgeId}/{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeItemsByUserAsync(
            [FromRoute] Guid fridgeId,
            [FromRoute] Guid userId,
            CancellationToken ct)
        {
            return Ok(await fridgeItemService.GetFridgeItemsByUserAsync(userId, fridgeId, ct));
        }

        [Route("{fridgeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeItemsByIdAsync(
            [FromRoute] Guid fridgeId,
            CancellationToken ct)
        {
            return Ok(await fridgeItemService.GetFridgeItemsByFridgeAsync(fridgeId, ct));
        }

        /// <summary>
        /// Add FridgeItem to fridge (for user).
        /// </summary>
        [Route("{fridgeId}/add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFridgeItemAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] AddFridgeItemRequest request,
            CancellationToken ct)
        {
            await fridgeItemService.AddFridgeItemAsync(fridgeId, request.FridgeItem, request.UserId, ct);
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Remove FridgeItem from Fridge.
        /// </summary>
        [Route("{fridgeId}/remove")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFridgeItemAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] RemoveFridgeItemRequest request,
            CancellationToken ct)
        {
            await fridgeItemService.RemoveFridgeItemAsync(request.FridgeItemId, request.UserId, fridgeId, ct);
            return NoContent();
        }

        /// <summary>
        /// Consume fridgeItem.
        /// </summary>
        [Route("{fridgeId}/consume")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsumeFridgeItemAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] ConsumeFridgeItemRequest request,
            CancellationToken ct)
        {
            await fridgeItemService.ConsumeFridgeItemAsync(request.FridgeItemId, request.UserId, fridgeId, request.AmountValue, ct);
            return NoContent();
        }

        /// <summary>
        /// Consume food products from given recipe.
        /// </summary>
        [Route("ConsumeRecipe")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsumeRecipeAsync(
            [FromBody] ConsumeRecipeRequest request,
            CancellationToken ct)
        {
            await fridgeItemService.ConsumeRecipeAsync(request.UserId, request.FridgeId, request.FoodProducts, ct);
            return NoContent();
        }

        [Route("{fridgeId}/waste")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WasteFridgeItemAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] WasteFridgeItemRequest request,
            CancellationToken ct)
        {
            await fridgeItemService.WasteFridgeItemAsync(request.FridgeItemId, request.UserId, fridgeId, request.Reason, ct);
            return NoContent();
        }

        [Route("{fridgeId}/waste-report/{year:int}/{month:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(MonthlyWasteReportDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMonthlyWasteReportAsync(
            [FromRoute] Guid fridgeId,
            [FromRoute] int year,
            [FromRoute] int month,
            CancellationToken ct)
        {
            return Ok(await fridgeItemService.GetMonthlyWasteReportAsync(fridgeId, year, month, ct));
        }
    }
}
