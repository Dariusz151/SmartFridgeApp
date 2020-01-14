using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Users.AddFridgeUser;
using SmartFridgeApp.API.Users.GetFridgeUsers;
using SmartFridgeApp.API.Users.RemoveFridgeUser;

namespace SmartFridgeApp.API.Users
{
    [Route("api/fridgeUsers")]
    [ApiController]
    public class FridgeUsersController : Controller
    {
        private readonly IMediator _mediator;

        public FridgeUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of users from given fridge
        /// </summary>
        [Route("{fridgeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<FridgeUserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFridgeUsersAsync(Guid fridgeId)
        {
            var users = await _mediator.Send(new GetFridgeUsersQuery(fridgeId));
            return Ok(users);
        }

        /// <summary>
        /// Add user to fridge.
        /// </summary>
        [Route("{fridgeId}/add")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFridgeUserAsync(
            [FromRoute]Guid fridgeId,
            [FromBody]AddFridgeUserRequest request)
        {
            await _mediator.Send(new AddFridgeUserCommand(fridgeId, request.User));

            //return userId ?
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Remove user from fridge.
        /// </summary>
        [Route("{fridgeId}/remove")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveFridgeUserAsync(
            [FromRoute]Guid fridgeId,
            [FromBody]RemoveFridgeUserRequest request)
        {
            await _mediator.Send(new RemoveFridgeUserCommand(fridgeId, request.UserId));
            
            return NoContent();
        }
    }
}
