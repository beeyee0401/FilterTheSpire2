using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Ascension;

namespace FilterTheSpire2.Code.Filters;

/// <param name="bossOption">The boss to match.</param>
/// <param name="actNum">The act to check. Ignored when <paramref name="isSecondBoss"/> is true, since only
/// the last act (currently always Act 3) can roll a second boss.</param>
/// <param name="isSecondBoss">If true, checks the A10 second-boss roll instead of the
/// act's normal (first) boss.</param>
public class BossFilter(BossOptions bossOption, int actNum, bool isSecondBoss = false) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        // The second boss only exists at A10
        if (isSecondBoss && request.AscensionLevel < AscensionLevel.DoubleBoss)
        {
            return true;
        }

        var rolledActs = ActGenerator.GetRolledActs(seed, request.AscensionLevel);
        var act = rolledActs[actNum - 1];

        return isSecondBoss
            ? act.RolledSecondBoss == bossOption
            : act.RolledBoss == bossOption;
    }
}