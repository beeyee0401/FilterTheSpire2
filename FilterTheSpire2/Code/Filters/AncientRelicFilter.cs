using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;

namespace FilterTheSpire2.Code.Filters;

public class AncientRelicFilter(Ancient selectedAncient, Enum? relicOption, int actNum) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var seedLong = RngHelper.GetSeedHash(seed);

        if (actNum == 1)
        {
            var neow = AncientFactory.GetAncient(Ancient.Neow, actNum);
            return relicOption != null && neow.CheckOptions(seedLong, relicOption);
        }

        if (actNum > 1)
        {
            var rollResult = ActGenerator.GetActRollResult(seed);

            if (rollResult.Ancients[actNum - 1] != selectedAncient)
            {
                return false;
            }

            if (relicOption == null)
            {
                return true;
            }

            var ancient = AncientFactory.GetAncient(selectedAncient, actNum);
            return ancient.CheckOptions(seedLong, relicOption);
        }

        return true;
    }
}