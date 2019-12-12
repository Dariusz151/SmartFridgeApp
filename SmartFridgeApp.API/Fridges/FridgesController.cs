using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Fridges.AddFridge;
using System.Net;
using System.Threading.Tasks;

namespace SmartFridgeApp.API.Fridges
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgesController : Controller
    {
        private readonly IMediator _mediator;

        public FridgesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register fridge.
        /// </summary>
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
