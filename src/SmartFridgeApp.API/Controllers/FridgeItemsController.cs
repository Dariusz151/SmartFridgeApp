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
        [Route("{fridgeId}/{memberId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeItemsByMemberAsync(
            [FromRoute] Guid fridgeId,
            [FromRoute] int memberId,
            CancellationToken ct)
        {
            return Ok(await fridgeItemService.GetFridgeItemsByMemberAsync(memberId, ct));
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
        /// Add FridgeItem to fridge (for member).
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
            await fridgeItemService.AddFridgeItemAsync(fridgeId, request.FridgeItem, request.MemberId, ct);
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
            await fridgeItemService.RemoveFridgeItemAsync(request.FridgeItemId, request.MemberId, fridgeId, ct);
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
            await fridgeItemService.ConsumeFridgeItemAsync(request.FridgeItemId, request.MemberId, fridgeId, request.AmountValue, ct);
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
            await fridgeItemService.ConsumeRecipeAsync(request.MemberId, request.FridgeId, request.FoodProducts, ct);
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
            await fridgeItemService.WasteFridgeItemAsync(request.FridgeItemId, request.MemberId, fridgeId, request.Reason, ct);
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

        /// <summary>
        /// Get items expiring soon for a fridge.
        /// </summary>
        [Route("{fridgeId}/expiring")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpiringItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpiringItemsAsync(
            [FromRoute] Guid fridgeId,
            [FromQuery] int days = 3,
            CancellationToken ct = default)
        {
            return Ok(await fridgeItemService.GetExpiringItemsAsync(fridgeId, days, ct));
        }

        /// <summary>
        /// Get the fridge's overall waste score (gamification).
        /// </summary>
        [Route("{fridgeId}/score")]
        [HttpGet]
        [ProducesResponseType(typeof(FridgeScoreDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeScoreAsync(
            [FromRoute] Guid fridgeId,
            CancellationToken ct)
        {
            return Ok(await fridgeItemService.GetFridgeScoreAsync(fridgeId, ct));
        }

        /// <summary>
        /// Get the fridge's shopping status (inventory tracker).
        /// </summary>
        [Route("{fridgeId}/shopping-status")]
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingStatusDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetShoppingStatusAsync(
            [FromRoute] Guid fridgeId,
            CancellationToken ct)
        {
            return Ok(await fridgeItemService.GetShoppingStatusAsync(fridgeId, ct));
        }
    }
}
