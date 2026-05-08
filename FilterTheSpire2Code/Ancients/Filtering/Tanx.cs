using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Tanx : AbstractAncient
{
    public Tanx()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Tanx>().Id.Entry;
        Ancient = Ancient.Tanx;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
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
        var options = list.Take(3).Select(o => RelicModelMappings.GetRelicModel(o)!.Id);
        return options.Contains(relic.Id);
    }
}