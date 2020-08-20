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
using SmartFridgeApp.Domain.SeedWork.Exceptions;

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
            [FromRoute]Guid fridgeId, 
            [FromRoute]Guid userId)
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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveFridgeItemAsync(
            [FromRoute]Guid fridgeId,
            [FromBody]RemoveFridgeItemRequest request)
        {
            try
            {
                await _mediator.Send(new RemoveFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId));
            }
            catch (InvalidFridgeIdException e)
            {
                return BadRequest("Invalid fridge id.");
            }
            catch (UserNotExistException e)
            {
                return BadRequest("User does not exist.");
            }
            catch (FridgeItemNotExistException e)
            {
                return BadRequest("Fridge item does not exist.");
            }
            
            return NoContent();
        }

        ///// <summary>
        ///// Consume fridgeItem.
        ///// </summary>
        [Route("{fridgeId}/consume")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ConsumeFridgeItemAsync(
            [FromRoute]Guid fridgeId,
            [FromBody]ConsumeFridgeItemRequest request)
        {
            try
            {
                await _mediator.Send(new ConsumeFridgeItemCommand(request.FridgeItemId, request.UserId, fridgeId, request.AmountValue));
            }
            catch (InvalidFridgeIdException e)
            {
                return BadRequest("Invalid fridge id.");
            }
            catch (UserNotExistException e)
            {
                return BadRequest("User does not exist.");
            }
            catch (FridgeItemNotExistException e)
            {
                return BadRequest("Fridge item does not exist.");
            }
            catch (DomainException domainException)
            {
                return BadRequest($"Can't consume fridge item. Error message: {domainException.Details}");
            }
            
            return NoContent();
        }
    }
}
