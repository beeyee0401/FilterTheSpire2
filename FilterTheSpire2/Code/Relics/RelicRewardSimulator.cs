using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Relics;

public static class RelicRewardSimulator
{
    public static Dictionary<RelicRarity, List<RelicOptions>> BuildFrontPullPools(ulong seed)
    {
        return new Dictionary<RelicRarity, List<RelicOptions>>
        {
            [RelicRarity.Common] = BuildShuffledPool(seed, RelicRarity.Common, RngHelper.RngCounters.CommonRelicPoolCounter),
            [RelicRarity.Uncommon] = BuildShuffledPool(seed, RelicRarity.Uncommon, RngHelper.RngCounters.UncommonRelicPoolCounter),
            [RelicRarity.Rare] = BuildShuffledPool(seed, RelicRarity.Rare, RngHelper.RngCounters.RareRelicPoolCounter),
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
        ulong seed,
        RelicRarity rarity,
        int counter)
    {
        var upfrontRng = RngHelper.GetRunRngType(seed, RunRngType.UpFront);
        upfrontRng.FastForwardCounter(counter);
        var pool = RelicRules.GetRelicPool(rarity).ToList();
        pool.UnstableShuffle(upfrontRng);
        return pool;
    }
}