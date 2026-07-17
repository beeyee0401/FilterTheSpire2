using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public class Tanx : AbstractAncient
{
    public Tanx()
    {
        Id = "TANX";
    }

    public override bool CheckOptions(ulong seed, Enum? relicOption)
    {
        if (relicOption is not TanxOptions relic)
        {
            return true;
        }
        var rng = RngHelper.GetEventRng(seed, Id!);
        var list = new List<TanxOptions>()
        {
            TanxOptions.Claws,
            TanxOptions.Crossbow,
            TanxOptions.IronClub,
            TanxOptions.MeatCleaver,
            TanxOptions.Sai,
            TanxOptions.SpikedGauntlets,
            TanxOptions.TanxsWhistle,
            TanxOptions.ThrowingAxe,
            TanxOptions.WarHammer,
            TanxOptions.TriBoomerang
        };
        
        list.UnstableShuffle(rng);
        var options = list.Take(3);
        return options.Contains(relic);
    }
}