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
    [Route("api/users")]
    [ApiController]
    public class FridgeUsersController : Controller
    {
        private readonly IMediator _mediator;

        public FridgeUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("{fridgeId}/users")]
        [HttpGet]
        [ProducesResponseType(typeof(List<FridgeUserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFridgeUsers(Guid fridgeId)
        {
            var users = await _mediator.Send(new GetFridgeUsersQuery(fridgeId));
            return Ok(users);
        }

        [Route("{fridgeId}/users")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFridgeUser(
            [FromRoute]Guid fridgeId,
            [FromBody]FridgeUserRequest request)
        {
            await _mediator.Send(new AddFridgeUserCommand(fridgeId, request.User));

            //return userId ?
            return Created(string.Empty, null);
        }

        [Route("{fridgeId}/users")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveFridgeUser(
            [FromRoute]Guid fridgeId,
            [FromBody]RemoveFridgeUserRequest request)
        {
            await _mediator.Send(new RemoveFridgeUserCommand(fridgeId, request.UserId));
            
            return NoContent();
        }
    }
}
