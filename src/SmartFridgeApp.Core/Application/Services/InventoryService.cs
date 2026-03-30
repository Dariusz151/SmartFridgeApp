using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.Core.Application.Services;

public class InventoryService(IKitchenInventoryRepository inventoryRepository) : IInventoryService
{
    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByKitchenAsync(Guid kitchenId, CancellationToken ct = default)
        => await inventoryRepository.GetActiveItemsByKitchenAsync(kitchenId, ct);

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByMemberAsync(Guid kitchenId, int memberId, CancellationToken ct = default)
        => await inventoryRepository.GetActiveItemsByMemberAsync(kitchenId, memberId, ct);

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByLocationAsync(Guid kitchenId, StorageLocation location, CancellationToken ct = default)
        => await inventoryRepository.GetActiveItemsByLocationAsync(kitchenId, location, ct);

    public async Task<IReadOnlyList<StockItemDto>> GetActiveItemsByTagAsync(Guid kitchenId, ItemTag tag, CancellationToken ct = default)
        => await inventoryRepository.GetActiveItemsByTagAsync(kitchenId, tag, ct);

    public async Task StockItemAsync(Guid kitchenId, int memberId, StockItemRequest request, CancellationToken ct = default)
    {
        var (inventory, version) = await inventoryRepository.LoadAsync(kitchenId, ct);

        // Try to merge with an existing matching item first
        var restockEvt = inventory.TryRestockExisting(
            request.FoodProductId,
            memberId,
            request.Amount,
            request.Unit,
            request.ExpirationDate,
            request.Location,
            request.VariantId);

        if (restockEvt is not null)
        {
            await inventoryRepository.AppendEventsAsync(kitchenId, version, [restockEvt], ct);
            return;
        }

        var evt = inventory.StockItem(
            request.FoodProductId,
            memberId,
            request.Amount,
            request.Unit,
            request.ExpirationDate,
            request.Note,
            request.Location,
            request.Tags,
            request.VariantId);

        await inventoryRepository.AppendEventsAsync(kitchenId, version, [evt], ct);
    }

    public async Task RemoveItemAsync(Guid kitchenId, Guid stockItemId, int memberId, CancellationToken ct = default)
    {
        var (inventory, version) = await inventoryRepository.LoadAsync(kitchenId, ct);
        var evt = inventory.RemoveItem(stockItemId, memberId);
        await inventoryRepository.AppendEventsAsync(kitchenId, version, [evt], ct);
    }

    public async Task ConsumeItemAsync(Guid kitchenId, ConsumeItemRequest request, CancellationToken ct = default)
    {
        var (inventory, version) = await inventoryRepository.LoadAsync(kitchenId, ct);

        var evt = inventory.ConsumeItem(
            request.StockItemId,
            request.MemberId,
            request.Amount,
            request.Unit);

        await inventoryRepository.AppendEventsAsync(kitchenId, version, [evt], ct);
    }

    public async Task ConsumeRecipeAsync(Guid kitchenId, int memberId, List<FoodProductDetails> ingredients, CancellationToken ct = default)
    {
        var (inventory, version) = await inventoryRepository.LoadAsync(kitchenId, ct);
        var events = inventory.ConsumeRecipe(memberId, ingredients);

        if (events.Count > 0)
            await inventoryRepository.AppendEventsAsync(kitchenId, version, events.Cast<object>().ToList(), ct);
    }

    public async Task WasteItemAsync(Guid kitchenId, WasteItemRequest request, CancellationToken ct = default)
    {
        var (inventory, version) = await inventoryRepository.LoadAsync(kitchenId, ct);
        var evt = inventory.WasteItem(request.StockItemId, request.MemberId, request.Reason);
        await inventoryRepository.AppendEventsAsync(kitchenId, version, [evt], ct);
    }

    public async Task<MonthlyWasteReportDto> GetMonthlyWasteReportAsync(Guid kitchenId, int year, int month, CancellationToken ct = default)
    {
        var items = await inventoryRepository.GetWastedItemsAsync(kitchenId, year, month, ct);

        return new MonthlyWasteReportDto
        {
            Year = year,
            Month = month,
            TotalItemsWasted = items.Count,
            Items = items.ToList()
        };
    }

    public async Task<IReadOnlyList<ExpiringItemDto>> GetExpiringItemsAsync(Guid kitchenId, int daysThreshold = 3, CancellationToken ct = default)
        => await inventoryRepository.GetExpiringItemsAsync(kitchenId, daysThreshold, ct);

    public async Task<KitchenScoreDto> GetKitchenScoreAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var (inventory, _) = await inventoryRepository.LoadAsync(kitchenId, ct);

        return new KitchenScoreDto
        {
            KitchenId = kitchenId,
            WasteScore = inventory.WasteScore,
            Rank = inventory.GetScoreRank()
        };
    }

    public async Task<ShoppingStatusDto> GetShoppingStatusAsync(Guid kitchenId, CancellationToken ct = default)
    {
        var (inventory, _) = await inventoryRepository.LoadAsync(kitchenId, ct);

        return new ShoppingStatusDto
        {
            KitchenId = kitchenId,
            ActiveItemCount = inventory.ActiveItemCount,
            AverageItemCount = Math.Round(inventory.AverageItemCount, 1),
            IsShoppingNeeded = inventory.IsShoppingNeeded()
        };
    }
}
