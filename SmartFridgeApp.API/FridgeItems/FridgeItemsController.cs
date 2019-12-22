using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.FridgeItems.AddFridgeItem;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.API.Fridges.AddFridge;
using SmartFridgeApp.API.Users.AddFridgeUser;

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

        //[Route("")]
        //[HttpPost]
        //[ProducesResponseType(typeof(FridgeDto), (int)HttpStatusCode.Created)]
        //public async Task<IActionResult> AddFridge([FromBody]AddFridgeRequest request)
        //{
        //    var fridge = await _mediator.Send(new AddFridgeCommand(request.Name, request.Address, request.Desc));

        //    return Created(string.Empty, fridge);
        //}

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
        ///// Remove user from fridge.
        ///// </summary>
        //[Route("{fridgeId}/remove")]
        //[HttpDelete]
        //[ProducesResponseType((int)HttpStatusCode.NoContent)]
        //public async Task<IActionResult> RemoveFridgeUser(
        //    [FromRoute]Guid fridgeId,
        //    [FromBody]RemoveFridgeUserRequest request)
        //{
        //    await _mediator.Send(new RemoveFridgeUserCommand(fridgeId, request.UserId));

        //    return NoContent();
        //}
    }
}
