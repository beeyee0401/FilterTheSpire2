using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.SeedSearcher;

namespace FilterTheSpire2.Code.Filters;

public class BossFilter(BossOptions bossOption, int actNum) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var rollResult = ActGenerator.GetActRollResult(seed);
        return rollResult.Bosses[actNum - 1] == bossOption;
    }
}