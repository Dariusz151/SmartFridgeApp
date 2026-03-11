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

public class FridgeMemberService(
    IFridgeMemberRepository fridgeMemberRepository,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IFridgeMemberService
{
    private static readonly string[] MemberColors =
    [
        "#00695c", "#1565c0", "#ad1457", "#e65100",
        "#4527a0", "#2e7d32", "#c62828", "#00838f",
        "#6a1b9a", "#ef6c00", "#283593", "#558b2f"
    ];

    // ── Reads (Dapper — lightweight selects from views) ──────────────────────

    public async Task<IEnumerable<FridgeDto>> GetMyFridgesAsync(string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "Name", "Address", "Desc", "WasteScore", "CreatedAt"
            FROM app.v_member_fridges
            WHERE "Email" = @Email
            """;
        return await connection.QueryAsync<FridgeDto>(sql, new { Email = email });
    }

    public async Task<IEnumerable<FridgeInviteDto>> GetPendingInvitesAsync(string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "FridgeId", "FridgeName", "InviterEmail", "InviterName", "InvitedAt"
            FROM app.v_pending_invites
            WHERE "Email" = @Email
            ORDER BY "InvitedAt" DESC
            """;
        return await connection.QueryAsync<FridgeInviteDto>(sql, new { Email = email });
    }

    public async Task<IEnumerable<FridgeMemberDto>> GetMembersAsync(Guid fridgeId, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "FridgeId", "Email", "Name", "MemberRole", "Status", "Color"
            FROM app.v_fridge_members_detail
            WHERE "FridgeId" = @FridgeId
            ORDER BY "MemberRole" DESC, "Status" ASC
            """;
        return await connection.QueryAsync<FridgeMemberDto>(sql, new { FridgeId = fridgeId });
    }

    // ── Writes (EF via repository + UnitOfWork) ──────────────────────

    public async Task AddCreatorAsync(Guid fridgeId, string email, CancellationToken ct = default)
    {
        var creator = FridgeMember.CreateCreator(fridgeId, email, MemberColors[0]);
        await fridgeMemberRepository.AddAsync(creator);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task InviteAsync(Guid fridgeId, string inviterEmail, string inviteeEmail, CancellationToken ct = default)
    {
        var inviterRole = await fridgeMemberRepository.GetMemberRoleAsync(fridgeId, inviterEmail);
        if (inviterRole != "Creator")
            throw new InvalidOperationException("Only the fridge creator can send invites.");

        // Check invitee has an account (AppUsers is not in EF — lightweight Dapper read)
        var connection = sqlConnectionFactory.GetOpenConnection();
        var inviteeExists = await connection.QuerySingleOrDefaultAsync<string>(
            """SELECT "Email" FROM app."AppUsers" WHERE "Email" = @Email""",
            new { Email = inviteeEmail });

        if (inviteeExists is null)
            throw new InvalidOperationException("No account found with this email.");

        var memberCount = await fridgeMemberRepository.CountMembersAsync(fridgeId);
        var color = MemberColors[memberCount % MemberColors.Length];

        var invited = FridgeMember.CreateInvited(fridgeId, inviteeEmail, color);
        await fridgeMemberRepository.AddAsync(invited);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task AcceptInviteAsync(int inviteId, string email, CancellationToken ct = default)
    {
        var member = await fridgeMemberRepository.GetPendingByIdAndEmailAsync(inviteId, email);
        member.Accept();
        await unitOfWork.CommitAsync(ct);
    }

    public async Task DeclineInviteAsync(int inviteId, string email, CancellationToken ct = default)
    {
        var member = await fridgeMemberRepository.GetPendingByIdAndEmailAsync(inviteId, email);
        fridgeMemberRepository.Remove(member);
        await unitOfWork.CommitAsync(ct);
    }
}
