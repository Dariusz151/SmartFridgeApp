using System;
using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Core.Domain.Inventory.Events;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Core.Domain.Inventory;

public class KitchenInventory
{
    public Guid Id { get; set; }

    private readonly Dictionary<Guid, StockItem> _activeItems = new();
    public IReadOnlyDictionary<Guid, StockItem> ActiveItems => _activeItems;

    public int WasteScore { get; private set; } = 1000;
    public int ActiveItemCount { get; private set; }
    public double AverageItemCount { get; private set; }
    public int SampleCount { get; private set; }

    // ── Scoring constants ──
    private const int ConsumeReward = 10;
    private const int WastePenalty = -25;
    private const int ExpiredPenalty = -5;

    // ── Shopping alert constants ──
    private const double SmoothingFactor = 0.15;
    private const double ShoppingThreshold = 0.5;
    private const int MinSamplesForAlert = 5;

    // ── Command methods (rich domain model — validate + return events) ──

    public ItemStocked StockItem(short foodProductId, int memberId, float amount, Unit unit, DateTimeOffset expirationDate, string note, StorageLocation location = StorageLocation.Fridge, List<ItemTag> tags = null, int? variantId = null)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than 0.", "InvalidAmount");

        if (expirationDate < DateTimeOffset.UtcNow.AddDays(-1))
            throw new DomainException("Cannot set past expiration date.", "InvalidExpirationDate");

        var evt = new ItemStocked(Guid.NewGuid(), foodProductId, memberId, amount, unit, expirationDate, note, location, tags ?? [], DateTimeOffset.UtcNow, variantId);
        Apply(evt);
        return evt;
    }

    public ItemRestocked TryRestockExisting(short foodProductId, int memberId, float amount, Unit unit,
        DateTimeOffset expirationDate, StorageLocation location, int? variantId = null)
    {
        var existing = _activeItems.Values.FirstOrDefault(i =>
            i.FoodProductId == foodProductId
            && i.MemberId == memberId
            && i.Unit == unit
            && i.Location == location
            && i.VariantId == variantId);

        if (existing is null) return null;

        var evt = new ItemRestocked(existing.Id, amount, expirationDate, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    public ItemConsumed ConsumeItem(Guid itemId, int memberId, float amount, Unit unit)
    {
        if (!_activeItems.TryGetValue(itemId, out var item))
            throw new DomainException("Item not found in inventory.", "ItemNotFound");

        if (item.MemberId != memberId)
            throw new DomainException("Item does not belong to this member.", "ItemOwnerMismatch");

        if (item.Unit != unit)
            throw new DomainException("Unit mismatch when consuming item.", "UnitMismatch");

        bool isFullyConsumed = amount >= item.Amount;
        float actualConsumed = isFullyConsumed ? item.Amount : amount;

        var evt = new ItemConsumed(itemId, memberId, actualConsumed, isFullyConsumed, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    public List<ItemConsumed> ConsumeRecipe(int memberId, List<FoodProductDetails> ingredients)
    {
        var events = new List<ItemConsumed>();

        foreach (var ingredient in ingredients)
        {
            var item = _activeItems.Values
                .FirstOrDefault(i => i.FoodProductId == ingredient.FoodProductId && i.MemberId == memberId);

            if (item is null) continue;

            bool isFullyConsumed = ingredient.AmountValue.Value >= item.Amount;
            float actualConsumed = isFullyConsumed ? item.Amount : ingredient.AmountValue.Value;

            var evt = new ItemConsumed(item.Id, memberId, actualConsumed, isFullyConsumed, DateTimeOffset.UtcNow);
            Apply(evt);
            events.Add(evt);
        }

        return events;
    }

    public ItemWasted WasteItem(Guid itemId, int memberId, string reason = null)
    {
        if (!_activeItems.TryGetValue(itemId, out var item))
            throw new DomainException("Item not found in inventory.", "ItemNotFound");

        if (item.MemberId != memberId)
            throw new DomainException("Item does not belong to this member.", "ItemOwnerMismatch");

        var evt = new ItemWasted(itemId, memberId, reason, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    public ItemRemoved RemoveItem(Guid itemId, int memberId)
    {
        if (!_activeItems.ContainsKey(itemId))
            throw new DomainException("Item not found in inventory.", "ItemNotFound");

        var evt = new ItemRemoved(itemId, memberId, DateTimeOffset.UtcNow);
        Apply(evt);
        return evt;
    }

    // ── Shopping detection ──

    public bool IsShoppingNeeded()
        => SampleCount >= MinSamplesForAlert
           && AverageItemCount > 0
           && ActiveItemCount < AverageItemCount * ShoppingThreshold;

    public string GetScoreRank() => WasteScore switch
    {
        >= 2000 => "Eco Champion",
        >= 1500 => "Food Saver",
        >= 1000 => "Responsible",
        >= 500 => "Needs Improvement",
        _ => "Food Waster"
    };

    // ── Apply methods (Marten calls these to rebuild state from events) ──

    public void Apply(ItemStocked e)
    {
        _activeItems[e.ItemId] = new StockItem(
            e.ItemId, e.FoodProductId, e.MemberId, e.Amount, e.Unit, e.ExpirationDate, e.Note, e.Location, e.Tags, e.StockedAt, e.VariantId);
        ActiveItemCount++;
        UpdateAverage();
    }

    public void Apply(ItemConsumed e)
    {
        if (_activeItems.TryGetValue(e.ItemId, out var item))
        {
            if (e.IsFullyConsumed)
            {
                _activeItems.Remove(e.ItemId);
                ActiveItemCount = Math.Max(0, ActiveItemCount - 1);
                UpdateAverage();
            }
            else
            {
                item.DecreaseAmount(e.AmountConsumed);
            }
        }
        WasteScore += ConsumeReward;
    }

    public void Apply(ItemWasted e)
    {
        _activeItems.Remove(e.ItemId);
        ActiveItemCount = Math.Max(0, ActiveItemCount - 1);
        WasteScore += WastePenalty;
        UpdateAverage();
    }

    public void Apply(ItemRemoved e)
    {
        _activeItems.Remove(e.ItemId);
        ActiveItemCount = Math.Max(0, ActiveItemCount - 1);
        UpdateAverage();
    }

    public void Apply(ItemExpired e)
    {
        WasteScore += ExpiredPenalty;
    }

    public void Apply(ItemRestocked e)
    {
        if (_activeItems.TryGetValue(e.ItemId, out var item))
        {
            item.IncreaseAmount(e.AddedAmount, e.NewExpirationDate);
        }
    }

    // ── Private helpers ──

    private void UpdateAverage()
    {
        SampleCount++;
        AverageItemCount = SampleCount == 1
            ? ActiveItemCount
            : SmoothingFactor * ActiveItemCount + (1 - SmoothingFactor) * AverageItemCount;
    }
}
