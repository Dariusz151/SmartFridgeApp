using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFridgeItemService
{
    Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByMemberAsync(int memberId, CancellationToken ct = default);
    Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByFridgeAsync(Guid fridgeId, CancellationToken ct = default);
    Task AddFridgeItemAsync(Guid fridgeId, AddFridgeItemDto fridgeItemDto, int memberId, CancellationToken ct = default);
    Task RemoveFridgeItemAsync(long fridgeItemId, int memberId, Guid fridgeId, CancellationToken ct = default);
    Task ConsumeFridgeItemAsync(long fridgeItemId, int memberId, Guid fridgeId, AmountValue amountValue, CancellationToken ct = default);
    Task ConsumeRecipeAsync(int memberId, Guid fridgeId, List<FoodProductDetails> foodProducts, CancellationToken ct = default);
    Task WasteFridgeItemAsync(long fridgeItemId, int memberId, Guid fridgeId, string reason = null, CancellationToken ct = default);
    Task<MonthlyWasteReportDto> GetMonthlyWasteReportAsync(Guid fridgeId, int year, int month, CancellationToken ct = default);
    Task<IEnumerable<ExpiringItemDto>> GetExpiringItemsAsync(Guid fridgeId, int daysThreshold = 3, CancellationToken ct = default);
    Task<FridgeScoreDto> GetFridgeScoreAsync(Guid fridgeId, CancellationToken ct = default);
    Task<ShoppingStatusDto> GetShoppingStatusAsync(Guid fridgeId, CancellationToken ct = default);
}
