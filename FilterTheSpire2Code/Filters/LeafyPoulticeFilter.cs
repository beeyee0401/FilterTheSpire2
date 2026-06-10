using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class LeafyPoulticeFilter(List<CardOptions> cardOptions) : CardTransformFilter(cardOptions, 2)
{
    protected override Rng GetTransformRng(uint seed)
    {
        return RngHelper.GetPlayerRngType(seed, PlayerRngType.Transformations);
    }
}