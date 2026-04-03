using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features.ShoppingList;
using SmartFridgeApp.Core.Application.Services;

namespace SmartFridgeApp.API.Controllers;

[Route("api/kitchens/{kitchenId}/shopping-list")]
[ApiController]
[Authorize]
public class ShoppingListController(IShoppingListService shoppingListService) : Controller
{
    private string GetUserEmail() =>
        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ShoppingListItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItemsAsync(Guid kitchenId, CancellationToken ct)
    {
        return Ok(await shoppingListService.GetItemsAsync(kitchenId, ct));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingListItemDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItemAsync(Guid kitchenId, [FromBody] AddShoppingListItemRequest request, CancellationToken ct)
    {
        var email = GetUserEmail();
        var item = await shoppingListService.AddItemAsync(kitchenId, request.Name, email, ct);
        return Created(string.Empty, item);
    }

    [HttpPost("{itemId:guid}/buy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuyItemAsync(Guid kitchenId, Guid itemId, CancellationToken ct)
    {
        var email = GetUserEmail();
        await shoppingListService.BuyItemAsync(kitchenId, itemId, email, ct);
        return Ok();
    }

    [HttpDelete("{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItemAsync(Guid kitchenId, Guid itemId, CancellationToken ct)
    {
        var email = GetUserEmail();
        await shoppingListService.RemoveItemAsync(kitchenId, itemId, email, ct);
        return NoContent();
    }
}
