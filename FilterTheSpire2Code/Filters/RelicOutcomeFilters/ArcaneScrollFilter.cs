using System.Collections.Immutable;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

public class ArcaneScrollFilter(List<CardOptions> cardOptions) : 
    BaseCardTransformFilter(cardOptions, 1)
{
    protected override Rng GetTransformRng(uint seed)
    {
        return RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    }

    protected override ImmutableArray<CardOptions> GetCardPool()
    {
        return CardRules.RareCardPools[FilterTheSpire2Config.Character];
    }
}