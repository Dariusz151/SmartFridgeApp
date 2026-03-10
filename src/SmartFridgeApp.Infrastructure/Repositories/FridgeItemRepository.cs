using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Infrastructure.FridgeItems;

public class FridgeItemRepository(SmartFridgeAppContext context) : IFridgeItemRepository
{
    public async Task<FridgeItem> GetByIdAndMemberAsync(long fridgeItemId, int memberId)
    {
        var item = await context.FridgeItems
            .Where(fi => fi.Id == fridgeItemId && fi.MemberId == memberId)
            .SingleOrDefaultAsync();

        if (item is null)
            throw new FridgeItemNotExistException("FridgeItem not found or does not belong to this member.");

        return item;
    }

    public async Task<FridgeItem> GetActiveByMemberAndProductAsync(int memberId, short foodProductId)
    {
        return await context.FridgeItems
            .Where(fi => fi.MemberId == memberId
                         && fi.FoodProductId == foodProductId
                         && !fi.IsConsumed
                         && !fi.IsWasted)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(FridgeItem fridgeItem)
    {
        await context.FridgeItems.AddAsync(fridgeItem);
    }

    public async Task DeleteAsync(long fridgeItemId, int memberId)
    {
        var item = await GetByIdAndMemberAsync(fridgeItemId, memberId);
        context.FridgeItems.Remove(item);
    }
}
