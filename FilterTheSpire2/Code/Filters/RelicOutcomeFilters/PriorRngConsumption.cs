namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

/// <summary>
/// Tracks how many RNG steps a Neow outcome filter consumes per stream,
/// so that slot-2 filters can fast-forward past slot-1's consumption.
/// </summary>
public record PriorRngConsumption(
    int RewardsRngSteps,
    int TransformationsRngSteps,
    int NicheRngSteps,
    int CombatPotionGenerationRngSteps = 0
)
{
    public static readonly PriorRngConsumption None = new(0, 0, 0, 0);
}