using FilterTheSpire2.FilterTheSpire2Code.Relics;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public abstract class BaseRelicFilter(RelicOptions relicOption) : IFilter
{
    protected abstract RelicRarity RelicRarity { get; }
    protected abstract int RelicCounter { get; }

    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var runRng = new RunRngSet(seed);
        var upfrontRng = new Rng(runRng.UpFront.Seed, RelicCounter);

        var list = RelicRules.GetRelicPool(RelicRarity).ToList();

        list.UnstableShuffle(upfrontRng);
        // Shop relics pull from the end of the list
        var option = RelicRarity == RelicRarity.Shop ? list.Last() : list.First();
        return option == relicOption;
    }
}