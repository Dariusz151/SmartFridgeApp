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
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.API.Controllers;

[Route("api/kitchens/{kitchenId:guid}/inventory")]
[ApiController]
[Authorize]
public class InventoryController(IInventoryService inventoryService) : Controller
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StockItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActiveItemsAsync(
        [FromRoute] Guid kitchenId,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetActiveItemsByKitchenAsync(kitchenId, ct));
    }

    [HttpGet("location/{location}")]
    [ProducesResponseType(typeof(IReadOnlyList<StockItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActiveItemsByLocationAsync(
        [FromRoute] Guid kitchenId,
        [FromRoute] StorageLocation location,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetActiveItemsByLocationAsync(kitchenId, location, ct));
    }

    [HttpGet("tag/{tag}")]
    [ProducesResponseType(typeof(IReadOnlyList<StockItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActiveItemsByTagAsync(
        [FromRoute] Guid kitchenId,
        [FromRoute] ItemTag tag,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetActiveItemsByTagAsync(kitchenId, tag, ct));
    }

    [HttpGet("member/{memberId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<StockItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActiveItemsByMemberAsync(
        [FromRoute] Guid kitchenId,
        [FromRoute] int memberId,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetActiveItemsByMemberAsync(memberId, ct));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StockItemAsync(
        [FromRoute] Guid kitchenId,
        [FromBody] AddStockItemApiRequest request,
        CancellationToken ct)
    {
        await inventoryService.StockItemAsync(kitchenId, request.MemberId, request.Item, ct);
        return Created(string.Empty, null);
    }

    [HttpDelete("{stockItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveItemAsync(
        [FromRoute] Guid kitchenId,
        [FromRoute] Guid stockItemId,
        [FromBody] RemoveItemRequest request,
        CancellationToken ct)
    {
        await inventoryService.RemoveItemAsync(kitchenId, stockItemId, request.MemberId, ct);
        return NoContent();
    }

    [HttpPost("consume")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConsumeItemAsync(
        [FromRoute] Guid kitchenId,
        [FromBody] ConsumeItemRequest request,
        CancellationToken ct)
    {
        await inventoryService.ConsumeItemAsync(kitchenId, request, ct);
        return NoContent();
    }

    [HttpPost("consume-recipe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConsumeRecipeAsync(
        [FromRoute] Guid kitchenId,
        [FromBody] ConsumeRecipeApiRequest request,
        CancellationToken ct)
    {
        await inventoryService.ConsumeRecipeAsync(kitchenId, request.MemberId, request.FoodProducts, ct);
        return NoContent();
    }

    [HttpPost("{stockItemId:guid}/waste")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> WasteItemAsync(
        [FromRoute] Guid kitchenId,
        [FromRoute] Guid stockItemId,
        [FromBody] WasteItemRequest request,
        CancellationToken ct)
    {
        request.StockItemId = stockItemId;
        await inventoryService.WasteItemAsync(kitchenId, request, ct);
        return NoContent();
    }

    [HttpGet("waste-report/{year:int}/{month:int}")]
    [ProducesResponseType(typeof(MonthlyWasteReportDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMonthlyWasteReportAsync(
        [FromRoute] Guid kitchenId,
        [FromRoute] int year,
        [FromRoute] int month,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetMonthlyWasteReportAsync(kitchenId, year, month, ct));
    }

    [HttpGet("expiring")]
    [ProducesResponseType(typeof(IReadOnlyList<ExpiringItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpiringItemsAsync(
        [FromRoute] Guid kitchenId,
        [FromQuery] int days = 3,
        CancellationToken ct = default)
    {
        return Ok(await inventoryService.GetExpiringItemsAsync(kitchenId, days, ct));
    }

    [HttpGet("score")]
    [ProducesResponseType(typeof(KitchenScoreDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetKitchenScoreAsync(
        [FromRoute] Guid kitchenId,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetKitchenScoreAsync(kitchenId, ct));
    }

    [HttpGet("shopping-status")]
    [ProducesResponseType(typeof(ShoppingStatusDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetShoppingStatusAsync(
        [FromRoute] Guid kitchenId,
        CancellationToken ct)
    {
        return Ok(await inventoryService.GetShoppingStatusAsync(kitchenId, ct));
    }
}

public class AddStockItemApiRequest
{
    public StockItemRequest Item { get; set; }
    public int MemberId { get; set; }
}

public class ConsumeRecipeApiRequest
{
    public int MemberId { get; set; }
    public List<SmartFridgeApp.Core.Domain.Shared.FoodProductDetails> FoodProducts { get; set; }
}
