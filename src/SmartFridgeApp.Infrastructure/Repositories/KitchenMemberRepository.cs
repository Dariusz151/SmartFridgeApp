using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Infrastructure.KitchenMembers;

public class KitchenMemberRepository(SmartFridgeAppContext context) : IKitchenMemberRepository
{
    public async Task AddAsync(KitchenMember member) =>
        await context.KitchenMembers.AddAsync(member);

    public async Task<KitchenMember> GetPendingByIdAndEmailAsync(int id, string email)
    {
        var member = await context.KitchenMembers
            .FirstOrDefaultAsync(fm => fm.Id == id && fm.Email == email && fm.Status == "Pending");

        if (member is null)
            throw new DomainException("Invite not found or already processed.", "InviteNotFound");

        return member;
    }

    public async Task<string> GetMemberRoleAsync(Guid kitchenId, string email)
    {
        var member = await context.KitchenMembers
            .FirstOrDefaultAsync(fm => fm.KitchenId == kitchenId && fm.Email == email && fm.Status == "Accepted");

        return member?.MemberRole;
    }

    public async Task<int> CountMembersAsync(Guid kitchenId) =>
        await context.KitchenMembers.CountAsync(fm => fm.KitchenId == kitchenId);

    public void Remove(KitchenMember member) =>
        context.KitchenMembers.Remove(member);
}
