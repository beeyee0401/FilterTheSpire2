using FilterTheSpire2.FilterTheSpire2Code.ActLocations;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

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