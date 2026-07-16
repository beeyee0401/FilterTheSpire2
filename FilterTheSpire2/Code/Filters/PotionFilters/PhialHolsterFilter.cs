using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.SeedSearcher;

namespace FilterTheSpire2.Code.Filters.PotionFilters;

public class PhialHolsterFilter(
    List<PotionOptions> potionsToMatch,
    int priorConsumptionSteps = 0) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        if (potionsToMatch.Count == 0)
        {
            return true;
        }

        var generatedPotions = PotionRewardSimulator.Generate(
            seed,
            new PhialHolsterPotionSource(priorConsumptionSteps));

        return potionsToMatch.All(generatedPotions.Contains);
    }
}