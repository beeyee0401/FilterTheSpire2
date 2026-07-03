using FilterTheSpire2.Code.ActLocations;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;

namespace FilterTheSpire2.Code.Filters;

public class ActLocationFilter(ActLocations.ActLocations actLocation, int actNum) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        if (!ActLocationRules.IsValidForAct(actNum, actLocation))
        {
            return true;
        }

        var actList = RngHelper.GetRandomActs(seed);
        return actLocation == actList[0];
    }
}