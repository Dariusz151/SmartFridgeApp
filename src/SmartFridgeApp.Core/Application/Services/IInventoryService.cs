using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByKitchenAsync(Guid kitchenId, CancellationToken ct = default);
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByMemberAsync(int memberId, CancellationToken ct = default);
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByLocationAsync(Guid kitchenId, StorageLocation location, CancellationToken ct = default);
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByTagAsync(Guid kitchenId, ItemTag tag, CancellationToken ct = default);
    Task StockItemAsync(Guid kitchenId, int memberId, StockItemRequest request, CancellationToken ct = default);
    Task RemoveItemAsync(Guid kitchenId, Guid stockItemId, int memberId, CancellationToken ct = default);
    Task ConsumeItemAsync(Guid kitchenId, ConsumeItemRequest request, CancellationToken ct = default);
    Task ConsumeRecipeAsync(Guid kitchenId, int memberId, List<FoodProductDetails> ingredients, CancellationToken ct = default);
    Task WasteItemAsync(Guid kitchenId, WasteItemRequest request, CancellationToken ct = default);
    Task<MonthlyWasteReportDto> GetMonthlyWasteReportAsync(Guid kitchenId, int year, int month, CancellationToken ct = default);
    Task<IReadOnlyList<ExpiringItemDto>> GetExpiringItemsAsync(Guid kitchenId, int daysThreshold = 3, CancellationToken ct = default);
    Task<KitchenScoreDto> GetKitchenScoreAsync(Guid kitchenId, CancellationToken ct = default);
    Task<ShoppingStatusDto> GetShoppingStatusAsync(Guid kitchenId, CancellationToken ct = default);
}
