using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Application.Services;

namespace SmartFridgeApp.API.Controllers
{
    [Route("api/fridgeUsers")]
    [ApiController]
    [Authorize]
    public class FridgeUsersController(IFridgeUserService fridgeUserService) : Controller
    {
        /// <summary>
        /// Get list of users from given fridge.
        /// </summary>
        [Route("{fridgeId}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeUserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFridgeUsersAsync(Guid fridgeId, CancellationToken ct)
        {
            return Ok(await fridgeUserService.GetFridgeUsersAsync(fridgeId, ct));
        }

        /// <summary>
        /// Add user to fridge.
        /// </summary>
        [Route("{fridgeId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFridgeUserAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] AddFridgeUserRequest request,
            CancellationToken ct)
        {
            await fridgeUserService.AddFridgeUserAsync(fridgeId, request.User, ct);
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Update user details by given id.
        /// </summary>
        [Route("{fridgeId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFridgeUserAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] UpdateFridgeUserRequest request,
            CancellationToken ct)
        {
            await fridgeUserService.UpdateFridgeUserAsync(request.UserId, request.Name, fridgeId, ct);
            return Ok();
        }

        /// <summary>
        /// Remove user from fridge.
        /// </summary>
        [Route("{fridgeId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFridgeUserAsync(
            [FromRoute] Guid fridgeId,
            [FromBody] RemoveFridgeUserRequest request,
            CancellationToken ct)
        {
            await fridgeUserService.RemoveFridgeUserAsync(fridgeId, request.UserId, ct);
            return NoContent();
        }
    }
}
