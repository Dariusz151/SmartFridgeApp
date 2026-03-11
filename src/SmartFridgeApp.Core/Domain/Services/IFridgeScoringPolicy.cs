namespace SmartFridgeApp.Core.Domain.Services;

/// <summary>
/// Domain service that encapsulates the scoring rules for a fridge's WasteScore.
/// Keeping this as a separate interface allows the scoring algorithm to evolve
/// independently without touching the Fridge aggregate.
/// </summary>
public interface IFridgeScoringPolicy
{
    /// <summary>Points awarded when a fridge item is consumed (positive).</summary>
    int CalculateConsumeReward();

    /// <summary>Points deducted when a fridge item is wasted (negative).</summary>
    int CalculateWastePenalty();

    /// <summary>Points deducted when an item expires without being consumed (negative).</summary>
    int CalculateExpiredItemPenalty();
}
