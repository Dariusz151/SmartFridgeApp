using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Domain.Entities;

namespace SmartFridgeApp.Core.Contracts.Repositories;

public interface IFridgeItemRepository
{
    Task<FridgeItem> GetByIdAndMemberAsync(long fridgeItemId, int memberId);
    Task<FridgeItem> GetActiveByMemberAndProductAsync(int memberId, short foodProductId);
    Task AddAsync(FridgeItem fridgeItem);
    Task DeleteAsync(long fridgeItemId, int memberId);
}
