namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

/// <summary>
/// Tracks how many RNG steps a Neow outcome filter consumes per stream,
/// so that slot-2 filters can fast-forward past slot-1's consumption.
/// </summary>
public record NeowRngConsumption(
    int RewardsRngSteps,
    int TransformationsRngSteps,
    int NicheRngSteps
)
{
    public static readonly NeowRngConsumption None = new(0, 0, 0);
}