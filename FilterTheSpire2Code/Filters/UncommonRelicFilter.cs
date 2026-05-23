using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class UncommonRelicFilter(RelicOptions relicOption) : BaseRelicFilter(relicOption)
{
    protected override RelicRarity RelicRarity => RelicRarity.Uncommon;
    protected override int RelicCounter => RngHelper.RngCounters.UncommonRelicPoolCounter;
}