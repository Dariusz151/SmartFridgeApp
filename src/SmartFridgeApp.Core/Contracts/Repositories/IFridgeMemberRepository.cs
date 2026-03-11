using System;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Contracts.Repositories;

public interface IFridgeMemberRepository
{
    Task AddAsync(FridgeMember member);
    Task<FridgeMember> GetPendingByIdAndEmailAsync(int id, string email);
    Task<string> GetMemberRoleAsync(Guid fridgeId, string email);
    Task<int> CountMembersAsync(Guid fridgeId);
    void Remove(FridgeMember member);
}
