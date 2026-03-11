namespace SmartFridgeApp.Core.Domain.Services;

/// <summary>
/// Default scoring policy with fixed point values.
/// Swap this implementation or make the values configurable as requirements evolve.
/// </summary>
public sealed class DefaultFridgeScoringPolicy : IFridgeScoringPolicy
{
    public int CalculateConsumeReward() => +10;

    public int CalculateWastePenalty() => -25;

    public int CalculateExpiredItemPenalty() => -5;
}
