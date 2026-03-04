using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFridgeItemService
{
    Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByUserAsync(Guid userId, Guid fridgeId, CancellationToken ct = default);
    Task<IEnumerable<FridgeItemDto>> GetFridgeItemsByFridgeAsync(Guid fridgeId, CancellationToken ct = default);
    Task AddFridgeItemAsync(Guid fridgeId, AddFridgeItemDto fridgeItemDto, Guid userId, CancellationToken ct = default);
    Task RemoveFridgeItemAsync(long fridgeItemId, Guid userId, Guid fridgeId, CancellationToken ct = default);
    Task ConsumeFridgeItemAsync(long fridgeItemId, Guid userId, Guid fridgeId, AmountValue amountValue, CancellationToken ct = default);
    Task ConsumeRecipeAsync(Guid userId, Guid fridgeId, List<FoodProductDetails> foodProducts, CancellationToken ct = default);
    Task WasteFridgeItemAsync(long fridgeItemId, Guid userId, Guid fridgeId, string reason = null, CancellationToken ct = default);
    Task<MonthlyWasteReportDto> GetMonthlyWasteReportAsync(Guid fridgeId, int year, int month, CancellationToken ct = default);
}
