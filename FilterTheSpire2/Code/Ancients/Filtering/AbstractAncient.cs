using FilterTheSpire2.Code.Ancients.Config;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public abstract class AbstractAncient
{
    protected string? Id;
    protected Ancient Ancient;

    public abstract bool CheckOptions(uint seed, RelicModel relic);
}