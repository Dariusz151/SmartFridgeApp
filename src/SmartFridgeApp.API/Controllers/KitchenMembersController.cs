using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Application.Services;

namespace SmartFridgeApp.API.Controllers
{
    [Route("api/Kitchens")]
    [ApiController]
    [Authorize]
    public class KitchenMembersController(IKitchenMemberService KitchenMemberService) : Controller
    {
        private string GetUserEmail() =>
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        [Route("{kitchenId}/members")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KitchenMemberDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMembersAsync([FromRoute] Guid kitchenId, CancellationToken ct)
        {
            return Ok(await KitchenMemberService.GetMembersAsync(kitchenId, ct));
        }

        [Route("{kitchenId}/invite")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteUserAsync(
            [FromRoute] Guid kitchenId, [FromBody] InviteUserRequest request, CancellationToken ct)
        {
            var inviterEmail = GetUserEmail();
            await KitchenMemberService.InviteAsync(kitchenId, inviterEmail, request.Email, ct);
            return Ok();
        }

        [Route("~/api/invites/pending")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KitchenInviteDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPendingInvitesAsync(CancellationToken ct)
        {
            var email = GetUserEmail();
            return Ok(await KitchenMemberService.GetPendingInvitesAsync(email, ct));
        }

        [Route("~/api/invites/{inviteId}/accept")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptInviteAsync([FromRoute] int inviteId, CancellationToken ct)
        {
            var email = GetUserEmail();
            await KitchenMemberService.AcceptInviteAsync(inviteId, email, ct);
            return Ok();
        }

        [Route("~/api/invites/{inviteId}/decline")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeclineInviteAsync([FromRoute] int inviteId, CancellationToken ct)
        {
            var email = GetUserEmail();
            await KitchenMemberService.DeclineInviteAsync(inviteId, email, ct);
            return Ok();
        }
    }
}
