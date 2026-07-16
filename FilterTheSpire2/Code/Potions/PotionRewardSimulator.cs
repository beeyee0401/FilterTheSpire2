using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Potions;

namespace FilterTheSpire2.Code.Potions;

public static class PotionRewardSimulator
{
    public static IReadOnlyList<PotionOptions> Generate(string seed, PotionSource source)
    {
        var rng = source.GetRng(RngHelper.GetSeedHash(seed));
        var pool = PotionRules.GetPotionPool().ToList();
        var generated = new List<PotionOptions>(source.Count);

        for (var i = 0; i < source.Count; i++)
        {
            var roll = rng.NextFloat();
            var rarity =
                roll <= 0.10000000149011612 ? PotionRarity.Rare :
                roll <= 0.3499999940395355 ? PotionRarity.Uncommon :
                PotionRarity.Common;

            var candidates = pool.Where(p => p.Rarity == rarity).ToList();
            var picked = rng.NextItem(candidates)!;

            generated.Add(picked.Potion);
            pool.Remove(picked);
        }

        return generated;
    }
}
