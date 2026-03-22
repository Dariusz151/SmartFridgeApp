using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Core.Application.Services;

public interface IKitchenMemberService
{
    Task<IEnumerable<KitchenDto>> GetMyKitchensAsync(string email, CancellationToken ct = default);
    Task AddCreatorAsync(Guid kitchenId, string email, CancellationToken ct = default);
    Task InviteAsync(Guid kitchenId, string inviterEmail, string inviteeEmail, CancellationToken ct = default);
    Task<IEnumerable<KitchenInviteDto>> GetPendingInvitesAsync(string email, CancellationToken ct = default);
    Task AcceptInviteAsync(int inviteId, string email, CancellationToken ct = default);
    Task DeclineInviteAsync(int inviteId, string email, CancellationToken ct = default);
    Task<IEnumerable<KitchenMemberDto>> GetMembersAsync(Guid kitchenId, CancellationToken ct = default);
}
