using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.API.Fridges.AddFridge;

namespace SmartFridgeApp.API.FridgeItems
{
    [Route("api/fridgeitems")]
    [ApiController]
    public class FridgeItemsController : Controller
    {
        private readonly IMediator _mediator;

        public FridgeItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(FridgeDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFridge([FromBody]AddFridgeRequest request)
        {
            var fridge = await _mediator.Send(new AddFridgeCommand(request.Name, request.Address, request.Desc));

            return Created(string.Empty, fridge);
        }
    }
}
