using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.FridgeItems.AddFridgeItem;
using SmartFridgeApp.API.FridgeItems.ConsumeFridgeItem;
using SmartFridgeApp.API.FridgeItems.GetFridgeItems;
using SmartFridgeApp.API.FridgeItems.RemoveFridgeItem;

namespace SmartFridgeApp.API.FridgeItems
{
    [Route("api/fridgeItems")]
    [ApiController]
    public class FridgeItemsController : Controller
    {
        private readonly IMediator _mediator;

        public FridgeItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("{fridgeId}/{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<FridgeItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFridgeItemsByUser(
            [FromRoute]Guid fridgeId, 
            [FromRoute]Guid userId)
        {
            var fridges = await _mediator.Send(new GetFridgeItemsQuery(userId, fridgeId));

            return Ok(fridges);
        }

        /// <summary>
        /// Add FridgeItem to fridge (for user).
        /// </summary>
        [Route("{fridgeId}/add")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFridgeItem(
            [FromRoute]Guid fridgeId,
            [FromBody]AddFridgeItemRequest request)
        {
            await _mediator.Send(new AddFridgeItemCommand(fridgeId, request.FridgeItem, request.UserId));
            
            return Created(string.Empty, null);
        }

        ///// <summary>
        ///// Consume FridgeItem by given AmountValue.
        ///// </summary>
        [Route("{fridgeId}/consume")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveFridgeItem(
            [FromRoute]Guid fridgeId,
            [FromBody]RemoveFridgeItemRequest request)
        {
            await _mediator.Send(new RemoveFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId));

            return NoContent();
        }

        ///// <summary>
        ///// Remove fridgeItem from fridge (by user).
        ///// </summary>
        [Route("{fridgeId}/consume")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ConsumeFridgeItem(
            [FromRoute]Guid fridgeId,
            [FromBody]ConsumeFridgeItemRequest request)
        {
            await _mediator.Send(new ConsumeFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId, request.AmountValue));

            return NoContent();
        }
    }
}
