using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters.PotionFilters;

public class PhialHolsterPotionSource(int priorConsumptionSteps = 0) : PotionSource(priorConsumptionSteps)
{
    public override int Count => 2;

    protected override Rng GetBaseRng(ulong seed) =>
        RngHelper.GetPlayerRngType(seed, PlayerRngType.NonCombatGeneration);
}