using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Relics;

public static class RelicRewardSimulator
{
    public static Dictionary<RelicRarity, List<RelicOptions>> BuildFrontPullPools(RunRngSet runRng)
    {
        return new Dictionary<RelicRarity, List<RelicOptions>>
        {
            [RelicRarity.Common] = BuildShuffledPool(runRng, RelicRarity.Common, RngHelper.RngCounters.CommonRelicPoolCounter),
            [RelicRarity.Uncommon] = BuildShuffledPool(runRng, RelicRarity.Uncommon, RngHelper.RngCounters.UncommonRelicPoolCounter),
            [RelicRarity.Rare] = BuildShuffledPool(runRng, RelicRarity.Rare, RngHelper.RngCounters.RareRelicPoolCounter),
        };
    }

    public static RelicOptions PullNextRolledRelic(
        Rng rewardsRng,
        Dictionary<RelicRarity, List<RelicOptions>> relicPools)
    {
        var rarity = RollRarity(rewardsRng);
        var pool = relicPools[rarity];

        var relic = pool[0];

        foreach (var relicPool in relicPools.Values)
        {
            relicPool.Remove(relic);
        }

        return relic;
    }

    public static RelicRarity RollRarity(Rng rng)
    {
        var roll = rng.NextFloat();

        return roll < 0.5f
            ? RelicRarity.Common
            : roll < 0.83f
                ? RelicRarity.Uncommon
                : RelicRarity.Rare;
    }

    private static List<RelicOptions> BuildShuffledPool(
        RunRngSet runRng,
        RelicRarity rarity,
        int counter)
    {
        var rng = new Rng(runRng.UpFront.Seed, counter);
        var pool = RelicRules.GetRelicPool(rarity).ToList();
        pool.UnstableShuffle(rng);
        return pool;
    }
}