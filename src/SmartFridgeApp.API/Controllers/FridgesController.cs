using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Application.Services;

namespace SmartFridgeApp.API.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    [Authorize]
    public class FridgesController(IFridgeService fridgeService, IFridgeMemberService fridgeMemberService) : Controller
    {
        private string GetUserEmail() =>
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFridgesAsync(CancellationToken ct)
        {
            var email = GetUserEmail();
            return Ok(await fridgeMemberService.GetMyFridgesAsync(email, ct));
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(FridgeDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFridgeAsync([FromBody] AddFridgeRequest request, CancellationToken ct)
        {
            var email = GetUserEmail();
            var fridge = await fridgeService.AddFridgeAsync(request.Name, request.Address, request.Desc, ct);
            await fridgeMemberService.AddCreatorAsync(fridge.Id, email, ct);
            return Created(string.Empty, fridge);
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFridgeAsync([FromBody] UpdateFridgeRequest request, CancellationToken ct)
        {
            await fridgeService.UpdateFridgeAsync(request.FridgeId, request.Name, request.Desc, ct);
            return Ok();
        }

        [Route("")]
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFridgeAsync([FromBody] DeleteFridgeRequest request, CancellationToken ct)
        {
            await fridgeService.DeleteFridgeAsync(request.FridgeId, ct);
            return NoContent();
        }
    }
}
