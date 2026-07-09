using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Relics;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Runs;

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

        var runRng = new RunRngSet(seed);

        var rewardsRng = RngHelper.GetPlayerRngType(
            runRng.Seed,
            PlayerRngType.Rewards);

        rewardsRng.FastForwardCounter(_rngConsumption.RewardsRngSteps);

        var relicPools = RelicRewardSimulator.BuildFrontPullPools(runRng);

        var generatedRelics = new List<RelicOptions>();
        for (var i = 0; i < generatedRelicCount; i++)
        {
            generatedRelics.Add(
                RelicRewardSimulator.PullNextRolledRelic(rewardsRng, relicPools));
        }

        for (var i = 0; i < relicsToMatch.Count; i++)
        {
            if (generatedRelics[i] != relicsToMatch[i])
            {
                return false;
            }
        }

        return true;
    }
}