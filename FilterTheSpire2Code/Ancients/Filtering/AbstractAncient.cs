using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public abstract class AbstractAncient
{
    protected string? Id;
    protected Ancient Ancient;

    public abstract bool CheckOptions(uint seed, RelicModel relic);
}