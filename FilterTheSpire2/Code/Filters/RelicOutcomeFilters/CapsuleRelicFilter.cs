using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Relics;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class CapsuleRelicFilter(
    IReadOnlyList<RelicOptions> relicsToMatch,
    int generatedRelicCount,
    RngConsumptionSteps? rngConsumption = null) : IFilter
{
    private RngConsumptionSteps? _rngConsumption = rngConsumption;

    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        _rngConsumption ??= RngConsumptionSteps.None;

        var seedLong = RngHelper.GetSeedHash(seed);

        var rewardsRng = RngHelper.GetPlayerRngType(
            seedLong,
            PlayerRngType.Rewards);

        rewardsRng.FastForwardCounter(_rngConsumption.RewardsRngSteps);

        var relicPools = RelicRewardSimulator.BuildFrontPullPools(seedLong);

        var generatedRelics = new List<RelicOptions>();
        for (var i = 0; i < generatedRelicCount; i++)
        {
            generatedRelics.Add(
                RelicRewardSimulator.PullNextRolledRelic(rewardsRng, relicPools));
        }

        var generatedRelicsToCheck = generatedRelics
            .Take(relicsToMatch.Count)
            .ToHashSet();

        return relicsToMatch.All(generatedRelicsToCheck.Contains);
    }
}