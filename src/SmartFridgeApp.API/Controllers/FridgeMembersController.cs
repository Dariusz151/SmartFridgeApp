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
    [Route("api/fridges")]
    [ApiController]
    [Authorize]
    public class FridgeMembersController(IFridgeMemberService fridgeMemberService) : Controller
    {
        private string GetUserEmail() =>
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        [Route("{fridgeId}/members")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeMemberDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMembersAsync([FromRoute] Guid fridgeId, CancellationToken ct)
        {
            return Ok(await fridgeMemberService.GetMembersAsync(fridgeId, ct));
        }

        [Route("{fridgeId}/invite")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InviteUserAsync(
            [FromRoute] Guid fridgeId, [FromBody] InviteUserRequest request, CancellationToken ct)
        {
            var inviterEmail = GetUserEmail();
            await fridgeMemberService.InviteAsync(fridgeId, inviterEmail, request.Email, ct);
            return Ok();
        }

        [Route("~/api/invites/pending")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FridgeInviteDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPendingInvitesAsync(CancellationToken ct)
        {
            var email = GetUserEmail();
            return Ok(await fridgeMemberService.GetPendingInvitesAsync(email, ct));
        }

        [Route("~/api/invites/{inviteId}/accept")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptInviteAsync([FromRoute] int inviteId, CancellationToken ct)
        {
            var email = GetUserEmail();
            await fridgeMemberService.AcceptInviteAsync(inviteId, email, ct);
            return Ok();
        }

        [Route("~/api/invites/{inviteId}/decline")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeclineInviteAsync([FromRoute] int inviteId, CancellationToken ct)
        {
            var email = GetUserEmail();
            await fridgeMemberService.DeclineInviteAsync(inviteId, email, ct);
            return Ok();
        }
    }
}
