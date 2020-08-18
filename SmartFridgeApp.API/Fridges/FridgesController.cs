using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Fridges.AddFridge;
using System.Net;
using System.Threading.Tasks;
using SmartFridgeApp.API.Fridges.DeleteFridge;
using SmartFridgeApp.API.Fridges.GetFridges;
using SmartFridgeApp.API.Fridges.UpdateFridge;

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
        /// Get all available fridges.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<FridgeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllFridgesAsync()
        {
            var fridges = await _mediator.Send(new GetFridgesQuery());

            return Ok(fridges);
        }

        /// <summary>
        /// Register fridge.
        /// </summary>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(FridgeDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddFridgeAsync([FromBody]AddFridgeRequest request)
        {
            var fridge = await _mediator.Send(new AddFridgeCommand(request.Name, request.Address, request.Desc));
            
            return Created(string.Empty, fridge);
        }

        /// <summary>
        /// Register fridge.
        /// </summary>
        [Route("")]
        [HttpPut]
        [ProducesResponseType(typeof(FridgeDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateFridgeAsync([FromBody]UpdateFridgeRequest request)
        {
            await _mediator.Send(new UpdateFridgeCommand(request.FridgeId, request.Name, request.Desc));

            return Ok();
        }

        /// <summary>
        /// Delete fridge.
        /// </summary>
        [Route("")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteFridgeAsync([FromBody]DeleteFridgeRequest request)
        {
            await _mediator.Send(new DeleteFridgeCommand(request.FridgeId));

            return NoContent();
        }
    }
}
