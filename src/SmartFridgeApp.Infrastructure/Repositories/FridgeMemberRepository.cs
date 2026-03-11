using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Infrastructure.FridgeMembers;

public class FridgeMemberRepository(SmartFridgeAppContext context) : IFridgeMemberRepository
{
    public async Task AddAsync(FridgeMember member) =>
        await context.FridgeMembers.AddAsync(member);

    public async Task<FridgeMember> GetPendingByIdAndEmailAsync(int id, string email)
    {
        var member = await context.FridgeMembers
            .FirstOrDefaultAsync(fm => fm.Id == id && fm.Email == email && fm.Status == "Pending");

        if (member is null)
            throw new DomainException("Invite not found or already processed.", "InviteNotFound");

        return member;
    }

    public async Task<string> GetMemberRoleAsync(Guid fridgeId, string email)
    {
        var member = await context.FridgeMembers
            .FirstOrDefaultAsync(fm => fm.FridgeId == fridgeId && fm.Email == email && fm.Status == "Accepted");

        return member?.MemberRole;
    }

    public async Task<int> CountMembersAsync(Guid fridgeId) =>
        await context.FridgeMembers.CountAsync(fm => fm.FridgeId == fridgeId);

    public void Remove(FridgeMember member) =>
        context.FridgeMembers.Remove(member);
}
