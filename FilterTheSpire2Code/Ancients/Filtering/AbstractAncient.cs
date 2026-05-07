using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public abstract class AbstractAncient
{
    protected string? Id;
    protected Ancient Ancient;

    public abstract bool CheckOptions(uint seed, RelicModel relic);

    protected Rng GetEventRng(uint seed)
    {
        return new Rng((uint) (seed + 1UL + (ulong) StringHelper.GetDeterministicHashCode(Id!)));
    }
}