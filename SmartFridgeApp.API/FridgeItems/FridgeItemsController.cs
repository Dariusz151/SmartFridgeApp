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
        public async Task<IActionResult> GetFridgeItemsByUserAsync(
            [FromRoute]int fridgeId, 
            [FromRoute]int userId)
        {
            var fridgeItems = await _mediator.Send(new GetFridgeItemsQuery(userId, fridgeId));

            return Ok(fridgeItems);
        }

        /// <summary>
        /// Add FridgeItem to fridge (for user).
        /// </summary>
        [Route("{fridgeId}/add")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFridgeItemAsync(
            [FromRoute]int fridgeId,
            [FromBody]AddFridgeItemRequest request)
        {
            await _mediator.Send(new AddFridgeItemCommand(fridgeId, request.FridgeItem, request.UserId));
            
            return Created(string.Empty, null);
        }

        ///// <summary>
        ///// Remove FridgeItem from Fridge.
        ///// </summary>
        [Route("{fridgeId}/remove")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveFridgeItemAsync(
            [FromRoute]int fridgeId,
            [FromBody]RemoveFridgeItemRequest request)
        {
            await _mediator.Send(new RemoveFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId));

            return NoContent();
        }

        ///// <summary>
        ///// Consume fridgeItem.
        ///// </summary>
        [Route("{fridgeId}/consume")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ConsumeFridgeItemAsync(
            [FromRoute]int fridgeId,
            [FromBody]ConsumeFridgeItemRequest request)
        {
            // TODO: Consuming doesnt work.


            await _mediator.Send(new ConsumeFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId, request.AmountValue));

            return NoContent();
        }
    }
}
