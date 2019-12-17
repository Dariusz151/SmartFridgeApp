using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartFridgeApp.API.Users
{
    [Route("api/users")]
    [ApiController]
    public class FridgeUsersController
    {
        private readonly IMediator _mediator;

        public FridgeUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> GetFridgeUsers(Guid fridgeId)
        {
            var users = await _mediator.Send(new GetFridgeUsersQuery(fridgeId));
        }

    }
}
