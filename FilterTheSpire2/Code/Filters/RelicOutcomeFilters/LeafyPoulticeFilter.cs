using System.Collections.Immutable;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class LeafyPoulticeFilter(List<CardOptions> cardOptions, PriorRngConsumption? slot1Consumption = null)
    : BaseCardTransformFilter(cardOptions, 2, slot1Consumption)
{
    protected override Rng GetTransformRng(ulong seed) =>
        RngHelper.GetPlayerRngType(seed, PlayerRngType.Transformations);

    protected override ImmutableArray<CardOptions> GetCardPool() =>
        CardRules.AvailableCardPools[FilterTheSpire2Config.Character];

    protected override void FastForward(Rng rng, PriorRngConsumption consumption) =>
        rng.FastForwardCounter(consumption.TransformationsRngSteps);
}