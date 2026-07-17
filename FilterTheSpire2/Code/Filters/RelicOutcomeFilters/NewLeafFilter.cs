using System.Collections.Immutable;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class NewLeafFilter(CardOptions cardOptions, RngConsumptionSteps? slot1Consumption = null) : 
    BaseCardTransformFilter([cardOptions], 1, slot1Consumption)
{
    public override RngConsumptionSteps RngConsumptionSteps => new(0, 0, 1); // 1 Niche call
    
    protected override Rng GetTransformRng(ulong seed) => 
        RngHelper.GetRunRngType(seed, RunRngType.Niche); 
    
    protected override ImmutableArray<CardOptions> GetCardPool() => 
        CardRules.AvailableCardPools[FilterTheSpire2Config.Character];
    
    protected override void FastForward(Rng rng, RngConsumptionSteps consumptionSteps) =>
        rng.FastForwardCounter(consumptionSteps.NicheRngSteps);
}