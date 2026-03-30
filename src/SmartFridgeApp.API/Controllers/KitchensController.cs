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
    [Route("api/Kitchens")]
    [ApiController]
    [Authorize]
    public class KitchensController(IKitchenService KitchenService, IKitchenMemberService KitchenMemberService) : Controller
    {
        private string GetUserEmail() =>
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KitchenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyKitchensAsync(CancellationToken ct)
        {
            var email = GetUserEmail();
            return Ok(await KitchenMemberService.GetMyKitchensAsync(email, ct));
        }

        [Route("all")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<KitchenDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFridgesAsync(CancellationToken ct)
        {
            return Ok(await KitchenService.GetKitchensAsync(ct));
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(KitchenDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddKitchenAsync([FromBody] AddKitchenRequest request, CancellationToken ct)
        {
            var email = GetUserEmail();
            var Kitchen = await KitchenService.AddKitchenWithCreatorAsync(request.Name, request.Address, request.Desc, email, ct);
            return Created(string.Empty, Kitchen);
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateKitchenAsync([FromBody] UpdateKitchenRequest request, CancellationToken ct)
        {
            await KitchenService.UpdateKitchenAsync(request.kitchenId, request.Name, request.Desc, ct);
            return Ok();
        }

        [Route("")]
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteKitchenAsync([FromBody] DeleteKitchenRequest request, CancellationToken ct)
        {
            await KitchenService.DeleteKitchenAsync(request.kitchenId, ct);
            return NoContent();
        }
    }
}
