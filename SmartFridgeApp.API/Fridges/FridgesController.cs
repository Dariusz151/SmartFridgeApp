using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Fridges.AddFridge;
using System.Net;
using System.Threading.Tasks;
using SmartFridgeApp.API.Fridges.DeleteFridge;
using SmartFridgeApp.API.Fridges.GetFridges;
using SmartFridgeApp.API.Fridges.UpdateFridge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(typeof(IEnumerable<FridgeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFridgesAsync()
         {
            return Ok(await _mediator.Send(new GetFridgesQuery()));
        }

        /// <summary>
        /// Register fridge.
        /// </summary>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(FridgeDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFridgeAsync([FromBody]UpdateFridgeRequest request)
        {
            await _mediator.Send(new UpdateFridgeCommand(request.FridgeId, request.Name, request.Desc));

            return Ok();
        }

        /// <summary>
        /// Delete fridge.
        /// </summary>
        [Route("")]
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFridgeAsync([FromBody]DeleteFridgeRequest request)
        {
            await _mediator.Send(new DeleteFridgeCommand(request.FridgeId));

            return NoContent();
        }
    }
}
