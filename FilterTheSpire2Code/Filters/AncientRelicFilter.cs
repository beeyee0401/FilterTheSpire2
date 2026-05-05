using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class AncientRelicFilter(RelicModel relicModel, int actNum) : IFilter
{
    private RelicModel _relicModel = relicModel;
    private int _actNum = actNum;

    public bool IsSeedValid(string seed)
    {
        return true;
    }
}