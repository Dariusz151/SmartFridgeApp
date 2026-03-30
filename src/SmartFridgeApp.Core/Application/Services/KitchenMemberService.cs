using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class KitchenMemberService(
    IKitchenMemberRepository KitchenMemberRepository,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IKitchenMemberService
{
    private static readonly string[] MemberColors =
    [
        "#00695c", "#1565c0", "#ad1457", "#e65100",
        "#4527a0", "#2e7d32", "#c62828", "#00838f",
        "#6a1b9a", "#ef6c00", "#283593", "#558b2f"
    ];

    // ── Reads (Dapper — lightweight selects from views) ──────────────────────

    public async Task<IEnumerable<KitchenDto>> GetMyKitchensAsync(string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "Name", "Address", "Desc", "CreatedAt"
            FROM app.v_member_kitchens
            WHERE "Email" = @Email
            """;
        return await connection.QueryAsync<KitchenDto>(sql, new { Email = email });
    }

    public async Task<IEnumerable<KitchenInviteDto>> GetPendingInvitesAsync(string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "kitchenId", "kitchenName", "InviterEmail", "InviterName", "InvitedAt"
            FROM app.v_pending_invites
            WHERE "Email" = @Email
            ORDER BY "InvitedAt" DESC
            """;
        return await connection.QueryAsync<KitchenInviteDto>(sql, new { Email = email });
    }

    public async Task<IEnumerable<KitchenMemberDto>> GetMembersAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "kitchenId", "Email", "Name", "MemberRole", "Status", "Color"
            FROM app.v_kitchen_members_detail
            WHERE "kitchenId" = @kitchenId
            ORDER BY "MemberRole" DESC, "Status" ASC
            """;
        return await connection.QueryAsync<KitchenMemberDto>(sql, new { kitchenId = kitchenId });
    }

    // ── Writes (EF via repository + UnitOfWork) ──────────────────────

    public async Task AddCreatorAsync(Guid kitchenId, string email, CancellationToken ct = default)
    {
        var creator = KitchenMember.CreateCreator(kitchenId, email, MemberColors[0]);
        await KitchenMemberRepository.AddAsync(creator);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task InviteAsync(Guid kitchenId, string inviterEmail, string inviteeEmail, CancellationToken ct = default)
    {
        var inviterRole = await KitchenMemberRepository.GetMemberRoleAsync(kitchenId, inviterEmail);
        if (inviterRole != "Creator")
            throw new InvalidOperationException("Only the Kitchen creator can send invites.");

        // Check invitee has an account (AppUsers is not in EF — lightweight Dapper read)
        var connection = sqlConnectionFactory.GetOpenConnection();
        var inviteeExists = await connection.QuerySingleOrDefaultAsync<string>(
            """SELECT "Email" FROM app."AppUsers" WHERE "Email" = @Email""",
            new { Email = inviteeEmail });

        if (inviteeExists is null)
            throw new InvalidOperationException("No account found with this email.");

        var memberCount = await KitchenMemberRepository.CountMembersAsync(kitchenId);
        var color = MemberColors[memberCount % MemberColors.Length];

        var invited = KitchenMember.CreateInvited(kitchenId, inviteeEmail, color);
        await KitchenMemberRepository.AddAsync(invited);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task AcceptInviteAsync(int inviteId, string email, CancellationToken ct = default)
    {
        var member = await KitchenMemberRepository.GetPendingByIdAndEmailAsync(inviteId, email);
        member.Accept();
        await unitOfWork.CommitAsync(ct);
    }

    public async Task DeclineInviteAsync(int inviteId, string email, CancellationToken ct = default)
    {
        var member = await KitchenMemberRepository.GetPendingByIdAndEmailAsync(inviteId, email);
        KitchenMemberRepository.Remove(member);
        await unitOfWork.CommitAsync(ct);
    }
}
