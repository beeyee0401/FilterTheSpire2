using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Relics;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.Code.Filters;

public class UncommonRelicFilter(RelicOptions relicOption) : BaseRelicFilter(relicOption)
{
    protected override RelicRarity RelicRarity => RelicRarity.Uncommon;
    protected override int RelicCounter => RngHelper.RngCounters.UncommonRelicPoolCounter;
}