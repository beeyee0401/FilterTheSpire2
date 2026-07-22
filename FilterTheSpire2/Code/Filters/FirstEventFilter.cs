using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Events;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;

namespace FilterTheSpire2.Code.Filters;

/// <param name="eventOption">The event to match as the act's first-rolled event.</param>
/// <param name="actNum">The act to check (1-3).</param>
public class FirstEventFilter(EventOptions eventOption, int actNum) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        // We can only deterministically check if the first event in Act 1 is an actual event
        // (vs combat, shop, or treasure room). Acts 2 and 3 uses the same RNG counter so we can't determine it
        if (actNum == 1)
        {
            var rng = RngHelper.GetRunRngType(seed, RunRngType.UnknownMapPoint);
            if (!EventRollSimulator.RollIsEvent(rng))
            {
                return false;
            }
        }
        var rollResult = ActGenerator.GetActRollResult(seed, request.AscensionLevel);
        return rollResult.FirstEvents[actNum - 1] == eventOption;
    }
}