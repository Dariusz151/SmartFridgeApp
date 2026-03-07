using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFridgeMemberService
{
    Task<IEnumerable<FridgeDto>> GetMyFridgesAsync(string email, CancellationToken ct = default);
    Task AddCreatorAsync(Guid fridgeId, string email, CancellationToken ct = default);
    Task InviteAsync(Guid fridgeId, string inviterEmail, string inviteeEmail, CancellationToken ct = default);
    Task<IEnumerable<FridgeInviteDto>> GetPendingInvitesAsync(string email, CancellationToken ct = default);
    Task AcceptInviteAsync(int inviteId, string email, CancellationToken ct = default);
    Task DeclineInviteAsync(int inviteId, string email, CancellationToken ct = default);
    Task<IEnumerable<FridgeMemberDto>> GetMembersAsync(Guid fridgeId, CancellationToken ct = default);
}
