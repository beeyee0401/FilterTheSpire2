using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class NewLeafFilter(CardOptions cardOptions) : CardTransformFilter([cardOptions], 1)
{
    protected override Rng GetTransformRng(uint seed)
    {
        return RngHelper.GetRunRngType(seed, RunRngType.Niche); 
    }
}