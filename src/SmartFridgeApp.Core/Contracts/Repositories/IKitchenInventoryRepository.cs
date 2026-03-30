using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Domain.Inventory;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Contracts.Repositories;

public interface IKitchenInventoryRepository
{
    Task<(KitchenInventory Aggregate, long Version)> LoadAsync(Guid kitchenId, CancellationToken ct = default);
    Task AppendEventsAsync(Guid kitchenId, long expectedVersion, IReadOnlyList<object> events, CancellationToken ct = default);

    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByKitchenAsync(Guid kitchenId, CancellationToken ct = default);
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByMemberAsync(Guid kitchenId, int memberId, CancellationToken ct = default);
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByLocationAsync(Guid kitchenId, StorageLocation location, CancellationToken ct = default);
    Task<IReadOnlyList<StockItemDto>> GetActiveItemsByTagAsync(Guid kitchenId, ItemTag tag, CancellationToken ct = default);
    Task<IReadOnlyList<ExpiringItemDto>> GetExpiringItemsAsync(Guid kitchenId, int daysThreshold, CancellationToken ct = default);
    Task<IReadOnlyList<WasteReportItemDto>> GetWastedItemsAsync(Guid kitchenId, int year, int month, CancellationToken ct = default);
}
