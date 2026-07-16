using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters.PotionFilters;

public abstract class PotionSource(int priorConsumptionSteps)
{
    public abstract int Count { get; }

    protected abstract Rng GetBaseRng(uint seed);

    public Rng GetRng(uint seed)
    {
        var rng = GetBaseRng(seed);
        if (priorConsumptionSteps > 0)
        {
            rng.FastForwardCounter(priorConsumptionSteps);
        }
        return rng;
    }
}