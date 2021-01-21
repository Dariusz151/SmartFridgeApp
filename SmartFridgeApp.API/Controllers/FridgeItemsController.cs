using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.FridgeItems;
using SmartFridgeApp.API.FridgeItems.AddFridgeItem;
using SmartFridgeApp.API.FridgeItems.ConsumeFridgeItem;
using SmartFridgeApp.API.FridgeItems.GetFridgeItems;
using SmartFridgeApp.API.FridgeItems.RemoveFridgeItem;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.API.Controllers
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
        [ProducesResponseType(typeof(IEnumerable<FridgeItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeItemsByUserAsync(
            [FromRoute]Guid fridgeId, 
            [FromRoute]Guid userId)
        {
            return Ok(await _mediator.Send(new GetFridgeItemsQuery(userId, fridgeId)));
        }

        [Route("{fridgeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeItemsByIdAsync(
             [FromRoute]Guid fridgeId)
        {
            return Ok(await _mediator.Send(new GetFridgeItemsByIdQuery(fridgeId)));
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
            [FromRoute]Guid fridgeId,
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFridgeItemAsync(
            [FromRoute]Guid fridgeId,
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsumeFridgeItemAsync(
            [FromRoute]Guid fridgeId,
            [FromBody]ConsumeFridgeItemRequest request)
        {
            await _mediator.Send(new ConsumeFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId, request.AmountValue));
           
            return NoContent();
        }
    }
}
