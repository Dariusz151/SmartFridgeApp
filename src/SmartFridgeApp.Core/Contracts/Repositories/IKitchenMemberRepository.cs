using System;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Contracts.Repositories;

public interface IKitchenMemberRepository
{
    Task AddAsync(KitchenMember member);
    Task<KitchenMember> GetPendingByIdAndEmailAsync(int id, string email);
    Task<string> GetMemberRoleAsync(Guid kitchenId, string email);
    Task<int> CountMembersAsync(Guid kitchenId);
    void Remove(KitchenMember member);
}
