using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters.PotionFilters;

public class LostCofferPotionSource(int priorConsumptionSteps = 0) : PotionSource(priorConsumptionSteps + 9)
{
    public override int Count => 1;

    protected override Rng GetBaseRng(uint seed) =>
        RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
}