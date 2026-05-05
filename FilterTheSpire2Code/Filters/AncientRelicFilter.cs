using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class AncientRelicFilter(RelicModel relicModel) : IFilter
{
    private RelicModel _relicModel = relicModel;

    public bool IsSeedValid(string seed)
    {
        return true;
    }
}