using System.Collections.Immutable;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class LeafyPoulticeFilter(List<CardOptions> cardOptions, NeowRngConsumption? slot1Consumption = null)
    : BaseCardTransformFilter(cardOptions, 2, slot1Consumption)
{
    public override NeowRngConsumption RngConsumption => new(0, 2, 0); // 2 Transformations calls

    protected override Rng GetTransformRng(uint seed) =>
        RngHelper.GetPlayerRngType(seed, PlayerRngType.Transformations);

    protected override ImmutableArray<CardOptions> GetCardPool() =>
        CardRules.AvailableCardPools[FilterTheSpire2Config.Character];

    protected override void FastForward(Rng rng, NeowRngConsumption consumption) =>
        rng.FastForwardCounter(consumption.TransformationsRngSteps);
}