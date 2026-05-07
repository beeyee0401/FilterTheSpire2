using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Nonupeipe : AbstractAncient
{
    public Nonupeipe()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Nonupeipe>().Id.Entry;
        Ancient = Ancient.Nonupeipe;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
        var rng = GetEventRng(seed);
        var list = new List<NonupeipeOptions>()
        {
            NonupeipeOptions.BlessedAntler,
            NonupeipeOptions.BrilliantScarf,
            NonupeipeOptions.DelicateFrond,
            NonupeipeOptions.DiamondDiadem,
            NonupeipeOptions.FurCoat,
            NonupeipeOptions.Glitter,
            NonupeipeOptions.JewelryBox,
            NonupeipeOptions.LoomingFruit,
            NonupeipeOptions.SignetRing,
            NonupeipeOptions.BeautifulBracelet
        };

        list.UnstableShuffle(rng);
        var options = list.Take(3).Select(o => RelicModelMappings.GetRelicModel(o)!.Id);
        return options.Contains(relic.Id);
    }
}