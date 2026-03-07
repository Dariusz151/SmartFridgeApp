using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class FridgeMemberService(ISqlConnectionFactory sqlConnectionFactory) : IFridgeMemberService
{
    private static readonly string[] MemberColors =
    [
        "#00695c", "#1565c0", "#ad1457", "#e65100",
        "#4527a0", "#2e7d32", "#c62828", "#00838f",
        "#6a1b9a", "#ef6c00", "#283593", "#558b2f"
    ];

    public async Task<IEnumerable<FridgeDto>> GetMyFridgesAsync(string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT f."Id", f."Name", f."Address", f."Desc"
            FROM app."Fridges" f
            INNER JOIN app."FridgeMembers" fm ON fm."FridgeId" = f."Id"
            WHERE fm."Email" = @Email AND fm."Status" = 'Accepted'
            """;
        var fridges = await connection.QueryAsync<FridgeDto>(sql, new { Email = email });
        return fridges.AsEnumerable();
    }

    public async Task AddCreatorAsync(Guid fridgeId, string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        await connection.ExecuteAsync(
            """
            INSERT INTO app."FridgeMembers" ("FridgeId", "Email", "MemberRole", "Status", "Color", "InvitedAt")
            VALUES (@FridgeId, @Email, 'Creator', 'Accepted', @Color, @Now)
            ON CONFLICT ("FridgeId", "Email") DO NOTHING
            """,
            new { FridgeId = fridgeId, Email = email, Color = MemberColors[0], Now = DateTime.UtcNow });

        // Also create a domain User so the user appears in user selector and can add items
        var name = await connection.QuerySingleOrDefaultAsync<string>(
            """SELECT "Name" FROM app."AppUsers" WHERE "Email" = @Email""",
            new { Email = email });

        await connection.ExecuteAsync(
            """
            INSERT INTO app."Users" ("Id", "Name", "Email", "FridgeId", "CreatedAt")
            VALUES (@Id, @Name, @Email, @FridgeId, @Now)
            ON CONFLICT ("Id") DO NOTHING
            """,
            new { Id = Guid.NewGuid(), Name = name ?? email, Email = email, FridgeId = fridgeId, Now = DateTime.UtcNow });
    }

    public async Task InviteAsync(Guid fridgeId, string inviterEmail, string inviteeEmail, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        // Check inviter is Creator of this fridge
        var inviterRole = await connection.QuerySingleOrDefaultAsync<string>(
            """SELECT "MemberRole" FROM app."FridgeMembers" WHERE "FridgeId" = @FridgeId AND "Email" = @Email AND "Status" = 'Accepted'""",
            new { FridgeId = fridgeId, Email = inviterEmail });

        if (inviterRole != "Creator")
            throw new InvalidOperationException("Only the fridge creator can send invites.");

        // Check invitee exists in AppUsers
        var inviteeExists = await connection.QuerySingleOrDefaultAsync<string>(
            """SELECT "Email" FROM app."AppUsers" WHERE "Email" = @Email""",
            new { Email = inviteeEmail });

        if (inviteeExists is null)
            throw new InvalidOperationException("No account found with this email.");

        // Count existing members for color assignment
        var memberCount = await connection.QuerySingleAsync<int>(
            """SELECT COUNT(*) FROM app."FridgeMembers" WHERE "FridgeId" = @FridgeId""",
            new { FridgeId = fridgeId });

        var color = MemberColors[memberCount % MemberColors.Length];

        await connection.ExecuteAsync(
            """
            INSERT INTO app."FridgeMembers" ("FridgeId", "Email", "MemberRole", "Status", "Color", "InvitedAt")
            VALUES (@FridgeId, @Email, 'Member', 'Pending', @Color, @Now)
            ON CONFLICT ("FridgeId", "Email") DO NOTHING
            """,
            new { FridgeId = fridgeId, Email = inviteeEmail, Color = color, Now = DateTime.UtcNow });
    }

    public async Task<IEnumerable<FridgeInviteDto>> GetPendingInvitesAsync(string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fm."Id", fm."FridgeId", f."Name" AS "FridgeName",
                   creator."Email" AS "InviterEmail",
                   COALESCE(au."Name", creator."Email") AS "InviterName",
                   fm."InvitedAt"
            FROM app."FridgeMembers" fm
            INNER JOIN app."Fridges" f ON f."Id" = fm."FridgeId"
            INNER JOIN app."FridgeMembers" creator
                ON creator."FridgeId" = fm."FridgeId" AND creator."MemberRole" = 'Creator'
            LEFT JOIN app."AppUsers" au ON au."Email" = creator."Email"
            WHERE fm."Email" = @Email AND fm."Status" = 'Pending'
            ORDER BY fm."InvitedAt" DESC
            """;
        var invites = await connection.QueryAsync<FridgeInviteDto>(sql, new { Email = email });
        return invites.AsEnumerable();
    }

    public async Task AcceptInviteAsync(int inviteId, string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        var affected = await connection.ExecuteAsync(
            """UPDATE app."FridgeMembers" SET "Status" = 'Accepted' WHERE "Id" = @Id AND "Email" = @Email AND "Status" = 'Pending'""",
            new { Id = inviteId, Email = email });

        if (affected == 0)
            throw new InvalidOperationException("Invite not found or already processed.");

        // Get the fridgeId for this invite and create a domain User so the user can add items
        var fridgeId = await connection.QuerySingleOrDefaultAsync<Guid?>(
            """SELECT "FridgeId" FROM app."FridgeMembers" WHERE "Id" = @Id""",
            new { Id = inviteId });

        if (fridgeId.HasValue)
        {
            var name = await connection.QuerySingleOrDefaultAsync<string>(
                """SELECT "Name" FROM app."AppUsers" WHERE "Email" = @Email""",
                new { Email = email });

            await connection.ExecuteAsync(
                """
                INSERT INTO app."Users" ("Id", "Name", "Email", "FridgeId", "CreatedAt")
                VALUES (@Id, @Name, @Email, @FridgeId, @Now)
                ON CONFLICT ("Id") DO NOTHING
                """,
                new { Id = Guid.NewGuid(), Name = name ?? email, Email = email, FridgeId = fridgeId.Value, Now = DateTime.UtcNow });
        }
    }

    public async Task DeclineInviteAsync(int inviteId, string email, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        var affected = await connection.ExecuteAsync(
            """DELETE FROM app."FridgeMembers" WHERE "Id" = @Id AND "Email" = @Email AND "Status" = 'Pending'""",
            new { Id = inviteId, Email = email });

        if (affected == 0)
            throw new InvalidOperationException("Invite not found or already processed.");
    }

    public async Task<IEnumerable<FridgeMemberDto>> GetMembersAsync(Guid fridgeId, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT fm."Id", fm."FridgeId", fm."Email",
                   COALESCE(au."Name", fm."Email") AS "Name",
                   fm."MemberRole", fm."Status", fm."Color"
            FROM app."FridgeMembers" fm
            LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email"
            WHERE fm."FridgeId" = @FridgeId AND fm."Status" = 'Accepted'
            ORDER BY fm."MemberRole" DESC, fm."InvitedAt"
            """;
        var members = await connection.QueryAsync<FridgeMemberDto>(sql, new { FridgeId = fridgeId });
        return members.AsEnumerable();
    }
}
