using System.Collections.Immutable;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

public class ArcaneScrollFilter(List<CardOptions> cardOptions, NeowRngConsumption? slot1Consumption = null)
    : BaseCardTransformFilter(cardOptions, 1, slot1Consumption)
{
    public override NeowRngConsumption RngConsumption => new(1, 0, 0); // 1 Rewards call

    protected override Rng GetTransformRng(uint seed) =>
        RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);

    protected override ImmutableArray<CardOptions> GetCardPool() =>
        CardRules.RareCardPools[FilterTheSpire2Config.Character];

    protected override void FastForward(Rng rng, NeowRngConsumption consumption) =>
        rng.FastForwardCounter(consumption.RewardsRngSteps);
}